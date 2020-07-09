using Cybtans.Proto.AST;
using Cybtans.Proto.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Cybtans.Proto.Generators.Typescript
{
    public class AngularClientGenerator
    {
        ProtoFile _proto;
        TsOutputOption _option;

        public AngularClientGenerator(ProtoFile proto, TsOutputOption option)
        {
            _proto = proto;
            _option = option;
        }

        public void GenerateCode()
        {
            Directory.CreateDirectory(_option.OutputDirectory);           

            var writer = new TsFileWriter(_option.OutputDirectory ?? Environment.CurrentDirectory);
            writer.Writer.Append("import { Injectable } from '@angular/core';\r\n");
            writer.Writer.Append("import { Observable, of } from 'rxjs';\r\n");
            writer.Writer.Append("import { HttpClient, HttpHeaders, HttpEvent } from '@angular/common/http';\r\n");
            writer.Writer.Append("import { @{IMPORT} } from \'./models\';");            

            writer.Writer.AppendLine();          

            HashSet<ITypeDeclaration> types = new HashSet<ITypeDeclaration>();

            foreach (var item in _proto.ImportedFiles)
            {
                writer.Writer.AppendLine();

                GenerateCode(item, writer.Writer, types);

                writer.Writer.AppendLine();
            }

            GenerateCode(_proto, writer.Writer, types);

            var importWriter = new CodeWriter();
            writer.Writer.AddWriter(importWriter, "IMPORT");

            int i = 0;
            foreach (var item in types)
            {
                if (i++ > 0)
                {
                    importWriter.Append(", ");
                }
                importWriter.Append(item.Name.Pascal());
            }

            writer.Save("services");
        }

        private void GenerateCode(ProtoFile item, CodeWriter writer, HashSet<ITypeDeclaration> types)
        {
            writer.AppendLine();

            foreach (var srv in item.Declarations.Where(x => x is ServiceDeclaration).Select(x => (ServiceDeclaration)x))
            {
                writer.AppendLine();

                CodeWriter methods = new CodeWriter();

                foreach (var rpc in srv.Rpcs)
                {
                    methods.AppendLine();

                    var options = rpc.Option;
                    var request = rpc.RequestType;
                    var response = rpc.ResponseType;

                    if (!request.IsBuildIn)
                        types.Add(request);

                    if (!response.IsBuildIn)
                        types.Add(response);

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
                    methods.Append($" : Observable<{responseType}>");

                    methods.Append(" {").AppendLine();

                    var body = methods.Append(' ', 2).Block(rpc.Name);
                                    

                    body.Append($"return this.http.{options.Method.ToLowerInvariant()}<{responseType}>(");
                    if (path != null)
                    {
                        body.AppendTemplate($"`{url}`", path.ToDictionary(x => x.Name, x => (object)$"${{request.{x.Name.Camel()}}}"));
                    }
                    else
                    {
                        body.Append($"`{url}`");
                    }

                    if (options.Method == "POST" || options.Method == "PUT")
                    {
                        body.Append(", request");
                    }

                    body.Append(", {").AppendLine();

                    if (srv.Option.RequiredAuthorization || options.RequiredAuthorization)
                    {
                        body.Append(' ', 4).Append("headers: this.headers.set('Authorization', 'Bearer')");
                    }
                    else
                    {
                        body.Append(' ', 4).Append("headers: this.headers");
                    }                    
                   

                    if ((options.Method == "GET" || options.Method == "DELETE") && request is MessageDeclaration msg)
                    {
                        if (path != null)
                        {
                            var queryFields = msg.Fields.Except(path);
                            if (queryFields.Any())
                            {
                                var queryObj = string.Join(", ", queryFields.Select(x => $"{x.Name.Camel()}: request.{x.Name.Camel()}"));

                                body.Append(",").AppendLine();
                                body.Append(' ', 4).Append($"params: {{ {queryObj} }}");
                            }
                        }
                        else
                        {
                            body.Append(",").AppendLine();
                            body.Append(' ', 4).Append($"params: request");
                        }
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

            }
        }

        string serviceTemplate =
@"@Injectable({
  providedIn: 'root'
})
export class @{NAME} {  

    private headers =  new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });

    constructor(private http: HttpClient){}
    @{METHODS}
}
";
    }
}

