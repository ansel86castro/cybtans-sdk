using Cybtans.Proto.AST;
using Cybtans.Proto.Example;
using Cybtans.Proto.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Cybtans.Proto.Generators.Typescript
{
    public class ClientGenerator
    {
        ProtoFile _proto;
        TsOutputOption _option;
        public ClientGenerator(ProtoFile proto, TsOutputOption option)
        {
            _proto = proto;
            _option = option;
        }

        public void GenerateCode()
        {
            Directory.CreateDirectory(_option.OutputDirectory);

            var writer = new TsFileWriter(_option.OutputDirectory ?? Environment.CurrentDirectory);
            writer.Writer.Append("import { @{IMPORT} } from \'./models\';");

            writer.Writer.AppendLine(2);

            writer.Writer.AppendTemplate(baseClientTemplate, new Dictionary<string, object>
            {
                ["SERVICE"] = _proto.Package.Name.Pascal()
            });

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

                    string url = $"this._options.baseUrl+`/{srv.Option.Prefix}";
                    List<FieldDeclaration> path = null;

                    if (options.Template != null)
                    {
                        url += $"/{options.Template.Replace("{", "@{")}";

                        path = request is MessageDeclaration ? ((MessageDeclaration)request).GetPathBinding(options.Template) : null;
                    }

                    url += "`";

                    methods.Append($"{rpc.Name.Camel()}");
                    if (request == PrimitiveType.Void)
                    {
                        methods.Append("()");
                    }
                    else
                    {
                        methods.Append($"(request:{request.GetTypeName()})");
                    }

                    var responseType = response != PrimitiveType.Void ? response.GetTypeName() : "ErrorInfo|void";
                    methods.Append($" : Promise<{responseType}>");

                    methods.Append(" {").AppendLine();

                    var body = methods.Append('\t', 1).Block(rpc.Name);

                    if (srv.Option.RequiredAuthorization || options.RequiredAuthorization)
                    {
                        body.Append($"let options:RequestInit = {{ method: '{options.Method}', headers: {{ ...this._headers, 'Authorization': 'Bearer'}}}};");
                    }
                    else
                    {
                        body.Append($"let options:RequestInit = {{ method: '{options.Method}', headers: this._headers}};");
                    }

                    body.AppendLine();

                    if (options.Method == "POST" || options.Method == "PUT")
                    {
                        body.Append("options.body = JSON.stringify(request);").AppendLine();
                    }

                    if (path != null)
                    {
                        body.AppendTemplate($"let endpoint = {url}", path.ToDictionary(x => x.Name, x => (object)$"${{request.{x.Name.Camel()}}}"));
                    }
                    else
                    {
                        body.Append($"let endpoint = {url}");
                    }

                    if ((options.Method == "GET" || options.Method == "DELETE") && request is MessageDeclaration msg)
                    {
                        if (path != null)
                        {
                            var queryFields = msg.Fields.Except(path);
                            if (queryFields.Any())
                            {
                                var queryObj = string.Join(",", queryFields.Select(x => $"{x.Name.Camel()}: request.{x.Name.Camel()}"));
                                body.Append($"+this.getQueryString({{ {queryObj}}})");
                            }
                        }
                        else
                        {
                            body.Append("+this.getQueryString(request)");
                        }
                    }

                    body.Append(";").AppendLine();

                    if (response != PrimitiveType.Void)
                    {
                        body.Append($"return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));");
                    }
                    else
                    {
                        body.Append($"return this._fetch(endpoint, options).then((response:Response) => this.ensureSuccess(response));");
                    }

                    body.AppendLine();

                    methods.Append("}");

                    methods.AppendLine();
                }

                writer.AppendTemplate(serviceTemplate, new Dictionary<string, object>
                {
                    ["SERVICE"] = item.Package.Name.Pascal(),
                    ["NAME"] = srv.Name.Pascal(),
                    ["METHODS"] = methods.ToString()
                });

            }
        }

        string baseClientTemplate =
@"type Fetch = (input: RequestInfo, init?: RequestInit)=> Promise<Response>;
type ErrorInfo = {status:number, statusText:string, text: string };

interface @{SERVICE}Options{
    baseUrl:string;
}

export class Base@{SERVICE}Service {
    protected _options:@{SERVICE}Options;
    protected _fetch:Fetch;
    protected _headers =  { 'Content-Type': 'application/json', 'Accept': 'application/json' };

    constructor(fetch:Fetch, options:@{SERVICE}Options){
        this._fetch = fetch;
        this._options = options;
    }

    protected getQueryString(data:any):string|undefined {
        if(!data)
            return '';
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

    protected getObject<T>(response:Response) : Promise<T>{
        let status = response.status;
        if(status >= 200 && status < 300 ){            
            return response.json();
        }     
        return response.text().then((text) => Promise.reject<T>({  status, statusText:response.statusText, text }));        
    }

    protected ensureSuccess(response:Response): Promise<ErrorInfo|void>{
        let status = response.status;
        if(status < 200 || status >= 300){
            return response.text().then((text) => Promise.reject<ErrorInfo>({  status, statusText:response.statusText, text }));        
        }
        return Promise.resolve();
    }
}
";

        string serviceTemplate =
@"export class @{NAME} extends Base@{SERVICE}Service {  

    constructor(fetch:Fetch, options:@{SERVICE}Options){
        super(fetch, options);        
    }
    @{METHODS}
}
";
    }
}
