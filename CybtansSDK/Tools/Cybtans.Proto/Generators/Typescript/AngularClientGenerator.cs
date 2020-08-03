using Cybtans.Proto.AST;
using Cybtans.Proto.Utils;
using System.Collections.Generic;
using System.Linq;

namespace Cybtans.Proto.Generators.Typescript
{
    public class AngularClientGenerator :BaseSingleFileGenerator
    {
        HashSet<string> _types = new HashSet<string>();
        TsOutputOption _modelsOptions;

        public AngularClientGenerator(ProtoFile proto, TsOutputOption modelsOptions , TsOutputOption option)
            :base(proto, option)
        {
            option.Filename ??= "services";
            _modelsOptions = modelsOptions;
        }

        public override void OnGenerationBegin(TsFileWriter writer)
        {
            writer.Writer.Append("import { Injectable } from '@angular/core';\r\n");
            writer.Writer.Append("import { Observable, of } from 'rxjs';\r\n");
            writer.Writer.Append("import { HttpClient, HttpHeaders, HttpEvent } from '@angular/common/http';\r\n");
            writer.Writer.Append($"import {{ @{{IMPORT}} }} from \'./{_modelsOptions.Filename}\';");

            writer.Writer.AppendLine();

            writer.Writer.Append(templateQueryFunction);

            writer.Writer.AppendLine();
        }

        public override void OnGenerationEnd(TsFileWriter writer)
        {
            var importWriter = new CodeWriter();
            writer.Writer.AddWriter(importWriter, "IMPORT");

            importWriter.AppendLine();            
            foreach (var item in _types)
            {                
                importWriter.Append(' ',1).Append(item).Append(",").AppendLine();
            }
        }

        protected override void GenerateCode(ProtoFile proto)
        {
            foreach (var srv in proto.Declarations.Where(x => x is ServiceDeclaration).Select(x => (ServiceDeclaration)x))
            {
                AddBlock(srv.Name, GenerateCode(srv));
            }
        }


        private string GenerateCode(ServiceDeclaration srv)
        {
            var writer = new CodeWriter();
            writer.AppendLine();

            CodeWriter methods = new CodeWriter();

            foreach (var rpc in srv.Rpcs)
            {
                methods.AppendLine();

                var options = rpc.Option;
                var request = rpc.RequestType;
                var response = rpc.ResponseType;

                if (!request.IsBuildIn)
                    _types.Add(request.Name.Pascal());

                if (!response.IsBuildIn)
                    _types.Add(response.Name.Pascal());

                string url = $"{srv.Option.Prefix}";
                List<FieldDeclaration> path = null;

                if (options.Template != null)
                {
                    url += $"/{options.Template.Replace("{", "@{")}";

                    path = request is MessageDeclaration ? ((MessageDeclaration)request).GetPathBinding(options.Template) : null;
                }

                methods.Append($"{rpc.Name.Camel()}");
                if (request == PrimitiveType.Void)
                {
                    methods.Append("()");
                }
                else
                {
                    methods.Append($"(request: {request.GetTypeName()})");
                }

                var responseType = response != PrimitiveType.Void ? response.GetTypeName() : "{}";
                methods.Append($": Observable<{responseType}>");

                methods.Append(" {").AppendLine();

                var body = methods.Append(' ', 2).Block(rpc.Name);


                body.Append($"return this.http.{options.Method.ToLowerInvariant()}<{responseType}>(");
                if (path != null)
                {
                    body.AppendTemplate($"`{url}", path.ToDictionary(x => x.Name, x => (object)$"${{request.{x.Name.Camel()}}}"));
                }
                else
                {
                    body.Append($"`{url}");
                }

                if ((options.Method == "GET" || options.Method == "DELETE") && request is MessageDeclaration msg)
                {
                    if (path != null)
                    {
                        var queryFields = msg.Fields.Except(path);
                        if (queryFields.Any())
                        {
                            var queryObj = string.Join(", ", queryFields.Select(x => $"{x.Name.Camel()}: request.{x.Name.Camel()}"));
                            body.Append($"${{ getQueryString({{ {queryObj} }}) }}");

                            //body.Append(",").AppendLine();
                            //body.Append(' ', 4).Append($"params: {{ {queryObj} }},");
                        }
                    }
                    else
                    {
                        body.Append($"${{ getQueryString(request) }}");
                        //var queryObj = string.Join(", ", msg.Fields.Select(x => $"{x.Name.Camel()}: request.{x.Name.Camel()}"));
                        //body.Append(' ', 4).Append($"params: {{ {queryObj} }},");
                    }
                }

                body.Append("`");

                if (options.Method == "POST" || options.Method == "PUT")
                {
                    body.Append(", request");
                }

                body.Append(", {").AppendLine();

                if (srv.Option.RequiredAuthorization || options.RequiredAuthorization)
                {
                    body.Append(' ', 4).Append("headers: this.headers.set('Authorization', 'Bearer'),");
                }
                else
                {
                    body.Append(' ', 4).Append("headers: this.headers,");
                }
            
                body.AppendLine();
                body.Append("});");

                body.AppendLine();

                methods.Append("}");

                methods.AppendLine();
            }

            writer.AppendTemplate(serviceTemplate, new Dictionary<string, object>
            {
                ["NAME"] = srv.Name.Pascal(),
                ["METHODS"] = methods.ToString()
            });

            return writer.ToString();
        }     

        string serviceTemplate =
@"@Injectable({
  providedIn: 'root',
})
export class @{NAME} {  

    private headers =  new HttpHeaders({
      'Content-Type': 'application/json',
       Accept: 'application/json',
    });

    constructor(private http: HttpClient) {}
    @{METHODS}

}
";

        string templateQueryFunction =
@"
function getQueryString(data:any): string|undefined {
  if(!data) return '';
  let args = [];
  for (const key in data) {
      if (data.hasOwnProperty(key)) {                
          const element = data[key];
          if(element){
              if(element instanceof Array){
                  element.forEach(e=>args.push(key+'='+ encodeURIComponent(e)) );
              }else{
                  args.push(key+'='+ encodeURIComponent(element));
              }
          }
      }
  }

  return args.length > 0 ? '?' + args.join('&') : '';   
}
";
    }
}

