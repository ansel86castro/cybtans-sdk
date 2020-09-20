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
            writer.Writer.Append("import { HttpClient, HttpHeaders, HttpEvent, HttpResponse } from '@angular/common/http';\r\n");
            writer.Writer.Append($"import {{ @{{IMPORT}} }} from \'./{_modelsOptions.Filename}\';");

            writer.Writer.AppendLine();

            writer.Writer.Append(templateQueryFunction).AppendLine();
            writer.Writer.Append(templateFormDataFunction).AppendLine();            
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

                string url = srv.Option.Prefix != null ? $"/{srv.Option.Prefix}" : "";
                List<FieldDeclaration> path = null;

                if (options.Template != null)
                {
                    url += $"/{options.Template.Replace("{", "@{")}";

                    path = request is MessageDeclaration ? ((MessageDeclaration)request).GetPathBinding(options.Template) : null;
                }

                if (rpc.Option.Description != null)
                {
                    methods.Append($"/** {rpc.Option.Description} */").AppendLine();
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
                
                var responseType =
                  response.HasStreams() ? "HttpResponse<Blob>" :
                  response == PrimitiveType.Void ? "{}" :
                  response.GetTypeName();

                methods.Append($": Observable<{responseType}>");

                methods.Append(" {").AppendLine();

                var body = methods.Append(' ', 2).Block(rpc.Name);

                body.Append($"return this.http.{options.Method.ToLowerInvariant()}");

                if (responseType != "HttpResponse<Blob>")
                {
                    body.Append($"<{responseType}>(");
                }
                else
                {
                    body.Append($"(");
                }

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
                            
                        }
                    }
                    else
                    {
                        body.Append($"${{ getQueryString(request) }}");                        
                    }
                }

                body.Append("`");

                if (options.Method == "POST" || options.Method == "PUT")
                {
                    body.Append(", ");
                    if (request.HasStreams())
                    {
                        if (request == PrimitiveType.Stream)
                        {
                            body.Append("getFormData({ blob: request })");
                        }
                        else
                        {
                            body.Append("getFormData(request)");
                        }
                    }
                    else
                    {
                        body.Append("request");
                    }                
                }              

                Dictionary<string, string> headers = new Dictionary<string, string>();
                if (srv.Option.RequiredAuthorization || options.RequiredAuthorization)
                {
                    headers.Add("Authorization", "Bearer");                    
                }
                
                if (!response.HasStreams())
                {
                    headers["Accept"] = "application/json";
                }

                if (!request.HasStreams() && (options.Method == "POST" || options.Method == "PUT"))
                {
                    headers["'Content-Type'"] = "application/json";
                }                

                if (headers.Any() || responseType == "HttpResponse<Blob>")
                {
                    body.Append(", {").AppendLine();

                    if (headers.Any())
                    {
                        var headerValues = headers.Select(x => $"{x.Key}: '{x.Value}'").Aggregate((x, y) => $"{x}, {y}");
                        body.Append(' ', 4).Append($"headers: new HttpHeaders({{ {headerValues} }}),").AppendLine();
                    }

                    if (responseType == "HttpResponse<Blob>")
                    {
                        body.Append(' ', 4).Append("observe: 'response',").AppendLine()
                            .Append(' ', 4).Append("responseType: 'blob',").AppendLine();
                    }

                    body.Append("}");
                }                

                body.Append(");");

                body.AppendLine();

                methods.Append("}");

                methods.AppendLine();
            }

            if (srv.Option.Description != null)
            {
                writer.Append($"/** {srv.Option.Description} */").AppendLine();
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

    constructor(private http: HttpClient) {}
    @{METHODS}
}
";

        string templateQueryFunction =
@"
function getQueryString(data:any): string|undefined {
  if(!data) return '';
  let args = [];
  for (let key in data) {
      if (data.hasOwnProperty(key)) {                
          let element = data[key];
          if(element !== undefined && element !== null && element !== ''){
              if(element instanceof Array){
                  element.forEach(e=>args.push(key + '=' + encodeURIComponent(e instanceof Date ? e.toJSON(): e)) );
              }else if(element instanceof Date){
                  args.push(key + '=' + encodeURIComponent(element.toJSON()));
              }else{
                  args.push(key + '=' + encodeURIComponent(element));
              }
          }
      }
  }

  return args.length > 0 ? '?' + args.join('&') : '';
}
";

        string templateFormDataFunction =
@"
function getFormData(data:any): FormData {
    let form = new FormData();
    if(!data)
        return form;
        
    for (let key in data) {
        if (data.hasOwnProperty(key)) {                
            let value = data[key];
            if(value !== undefined && value !== null && value !== ''){
                if (value instanceof Date){
                    form.append(key, value.toJSON());
                }else if(typeof value === 'number' || typeof value === 'bigint' || typeof value === 'boolean'){
                    form.append(key, value.toString());
                }else if(value instanceof File){
                    form.append(key, value, value.name);
                }else if(value instanceof Blob){
                    form.append(key, value, 'blob');
                }else if(typeof value ==='string'){
                    form.append(key, value);
                }else{
                    throw new Error(`value of ${key} is not supported for multipart/form-data upload`);
                }
            }
        }
    }
    return form;
}
";
    }
}

