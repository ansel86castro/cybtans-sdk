using Cybtans.Proto.AST;
using Cybtans.Proto.Example;
using Cybtans.Proto.Generators.CSharp;
using Cybtans.Proto.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Cybtans.Proto.Generators.Typescript
{
    public class ClientGenerator : BaseSingleFileGenerator
    {
        HashSet<string> _types = new HashSet<string>();
        TsOutputOption _modelsOptions;

        public ClientGenerator(ProtoFile proto, TsOutputOption modelsOptions , TsOutputOption option)
            :base(proto, option)
        {
            option.Filename ??= "services";
            _modelsOptions = modelsOptions;
        }

        public override void OnGenerationBegin(TsFileWriter writer)
        {
            writer.Writer.Append($"import {{ @{{IMPORT}} }} from \'./{_modelsOptions.Filename}\';");
            writer.Writer.AppendLine(2);
        }

        public override void OnGenerationEnd(TsFileWriter writer)
        {
            var importWriter = new CodeWriter();
            writer.Writer.AddWriter(importWriter, "IMPORT");
            
            importWriter.AppendLine();            
            foreach (var item in _types)
            {
                importWriter.Append(' ', 1).Append(item).Append(",").AppendLine();
            }
        }

        protected override void GenerateCode(ProtoFile proto)
        {
            AddBlock(proto.Package.Name.Pascal(), TemplateProcessor.ProcessDictionary(baseClientTemplate,
                 new Dictionary<string, object>
                 {
                     ["SERVICE"] = proto.Package.Name.Pascal()
                 }));

            foreach (var srv in proto.Declarations.Where(x => x is ServiceDeclaration).Select(x => (ServiceDeclaration)x))
            {
                AddBlock(srv.Name, GenerateCode(srv, proto));
            }
        }      

        private string GenerateCode(ServiceDeclaration srv, ProtoFile proto)
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

                var responseType =
                    response.HasStreams() ? "Blob" :
                    response == PrimitiveType.Void ? "ErrorInfo|void" :
                    response.GetTypeName();

                methods.Append($" : Promise<{responseType}>");

                methods.Append(" {").AppendLine();

                var body = methods.Append('\t', 1).Block(rpc.Name);

                var headers = new Dictionary<string, string>();

                if (!response.HasStreams())
                {
                    headers["Accept"] = "application/json";
                }

                if (srv.Option.RequiredAuthorization || options.RequiredAuthorization)
                {
                    headers["Authorization"] = "Bearer";
                }
                if (!request.HasStreams() && (options.Method == "POST" || options.Method == "PUT"))
                {
                    headers["'Content-Type'"] = "application/json";
                }

                var headersString =string.Join(", ",headers.Select(x => $"{x.Key}: '{x.Value}'"));
                body.Append($"let options:RequestInit = {{ method: '{options.Method}', headers: {{ {headersString} }}}};");
                body.AppendLine();

                if (options.Method == "POST" || options.Method == "PUT")
                {
                    if (request.HasStreams())
                    {
                        body.Append("options.body = this.getFormData(request);").AppendLine();
                    }
                    else
                    {
                        body.Append("options.body = JSON.stringify(request);").AppendLine();
                    }
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
                
                if(response.HasStreams())
                {
                    body.Append($"return this._fetch(endpoint, options).then((response:Response) => this.getBlob(response));");
                }
                else if(response == PrimitiveType.Void)
                {
                    body.Append($"return this._fetch(endpoint, options).then((response:Response) => this.ensureSuccess(response));");
                }
                else
                {
                    body.Append($"return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));");
                }

                body.AppendLine();

                methods.Append("}");

                methods.AppendLine();
            }

            writer.AppendTemplate(serviceTemplate, new Dictionary<string, object>
            {
                ["SERVICE"] = proto.Package.Name.Pascal(),
                ["NAME"] = srv.Name.Pascal(),
                ["METHODS"] = methods.ToString()
            });

            return writer.ToString();
        }     

        string baseClientTemplate =
@"export type Fetch = (input: RequestInfo, init?: RequestInit)=> Promise<Response>;
export type ErrorInfo = {status:number, statusText:string, text: string };

export interface @{SERVICE}Options{
    baseUrl:string;
}

class Base@{SERVICE}Service {
    protected _options:@{SERVICE}Options;
    protected _fetch:Fetch;    

    constructor(fetch:Fetch, options:@{SERVICE}Options){
        this._fetch = fetch;
        this._options = options;
    }

    protected getQueryString(data:any): string|undefined {
        if(!data)
            return '';
        let args = [];
        for (let key in data) {
            if (data.hasOwnProperty(key)) {                
                let element = data[key];
                if(element !== undefined && element !== null && element !== ''){
                    if(element instanceof Array){
                        element.forEach(e=>args.push(key + '=' + encodeURIComponent(e)) );
                    }else{
                        args.push(key + '=' + encodeURIComponent(element));
                    }
                }
            }
        }

       return args.length > 0 ? '?' + args.join('&') : '';    
    }

    protected getFormData(data:any): FormData {
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

    protected getObject<T>(response:Response): Promise<T>{
        let status = response.status;
        if(status >= 200 && status < 300 ){            
            return response.json();
        }     
        return response.text().then((text) => Promise.reject<T>({  status, statusText:response.statusText, text }));        
    }

     protected getBlob(response:Response): Promise<Blob>{
        let status = response.status;
        if(status >= 200 && status < 300 ){             
            return response.blob();
        }     
        return response.text().then((text) => Promise.reject<Blob>({  status, statusText:response.statusText, text }));
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
