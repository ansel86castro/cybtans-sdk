using Cybtans.Proto.AST;
using Cybtans.Proto.Options;
using Cybtans.Proto.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Cybtans.Proto.Generators.CSharp
{
    public class WebApiControllerGenerator : FileGenerator<WebApiControllerGeneratorOption>
    {
        protected ServiceGenerator _serviceGenerator;
        protected TypeGenerator _typeGenerator;      
        public WebApiControllerGenerator(ProtoFile proto, WebApiControllerGeneratorOption option,
         ServiceGenerator serviceGenerator, TypeGenerator typeGenerator) : base(proto, option)
        {
            _serviceGenerator = serviceGenerator;
            _typeGenerator = typeGenerator;            
        }

        public override void GenerateCode()
        {
            Directory.CreateDirectory(_option.OutputPath);

            foreach (var item in _serviceGenerator.Services)
            {              
                var srvInfo = item.Value;

                GenerateController(srvInfo);
            }
        }

        protected virtual void GenerateController(ServiceGenInfo srvInfo)
        {           
            var writer = CreateWriter($"{Proto.Option.Namespace}.{_option.Namespace ?? "Controllers"}");

            writer.Usings.Append($"using {_serviceGenerator.Namespace};").AppendLine();
            writer.Usings.Append($"using {_typeGenerator.Namespace};").AppendLine();
            GenerateControllerInternal(srvInfo, writer);
        }

        protected void GenerateControllerInternal(ServiceGenInfo srvInfo, CsFileWriter writer)
        {
            var srv = srvInfo.Service;
            writer.Usings.Append("using System.Collections.Generic;").AppendLine();
            writer.Usings.Append("using System.Threading.Tasks;").AppendLine();
            writer.Usings.Append("using Microsoft.AspNetCore.Http;").AppendLine();
            writer.Usings.Append("using Microsoft.AspNetCore.Mvc;").AppendLine();
            writer.Usings.Append("using Cybtans.AspNetCore;").AppendLine();
            var clsWriter = writer.Class;

            if (srv.Option.RequiredAuthorization || srv.Option.AllowAnonymous ||
               srv.Rpcs.Any(x => x.Option.RequiredAuthorization || x.Option.AllowAnonymous))
            {
                writer.Usings.Append("using Microsoft.AspNetCore.Authorization;").AppendLine();
            }

            AddAutorizationAttribute(srv.Option, clsWriter);
            
            clsWriter.Append($"[Route(\"{srv.Option.Prefix}\")]").AppendLine();
            clsWriter.Append("[ApiController]").AppendLine();
            clsWriter.Append($"public partial class {srvInfo.Name}Controller : ControllerBase").AppendLine();

            clsWriter.Append("{").AppendLine();
            clsWriter.Append('\t', 1);

            var bodyWriter = clsWriter.Block("BODY");

            bodyWriter.Append($"private readonly I{srvInfo.Name} _service;").AppendLine().AppendLine();

            #region Constructor

            bodyWriter.Append($"public {srvInfo.Name}Controller(I{srvInfo.Name} service)").AppendLine();
            bodyWriter.Append("{").AppendLine();
            bodyWriter.Append('\t', 1).Append("_service = service;").AppendLine();
            bodyWriter.Append("}").AppendLine();

            #endregion

            foreach (var rpc in srv.Rpcs)
            {
                var options = rpc.Option;
                var request = rpc.RequestType;
                var response = rpc.ResponseType;
                var rpcName = _serviceGenerator.GetRpcName(rpc);
                string template = options.Template != null ? $"(\"{options.Template}\")" : "";

                bodyWriter.AppendLine();

                AddAutorizationAttribute(options, bodyWriter);

                AddRequestMethod(bodyWriter, options, template);

                bodyWriter.AppendLine();
                
                if (request.HasStreams())
                {
                    bodyWriter.Append("[DisableFormValueModelBinding]").AppendLine();
                }

                bodyWriter.Append($"public {response.GetControllerReturnTypeName()} {rpcName}").Append("(");
                var parametersWriter = bodyWriter.Block($"PARAMS_{rpc.Name}");
                bodyWriter.Append($"{GetRequestBinding(options.Method, request)}{request.GetRequestTypeName("__request")})").AppendLine()
                    .Append("{").AppendLine()
                    .Append('\t', 1);

                var methodWriter = bodyWriter.Block($"METHODBODY_{rpc.Name}");

                bodyWriter.AppendLine().Append("}").AppendLine();

                if (options.Template != null)
                {
                    var path = request is MessageDeclaration ? _typeGenerator.GetMessageInfo(request).GetPathBinding(options.Template) : null;
                    if (path != null)
                    {                        
                        foreach (var field in path)
                        {
                            parametersWriter.Append($"{field.Type} {field.Field.Name}, ");
                            methodWriter.Append($"__request.{field.Name} = {field.Field.Name};").AppendLine();
                        }
                    }
                }

                if (response.HasStreams())
                {
                    var contentType = options.StreamOptions?.ContentType ?? "application/octet-stream";
                    var fileName = options.StreamOptions?.Name;
                    fileName = fileName != null ? $"\"{fileName}\"" : "Guid.NewGuid().ToString()";

                    methodWriter.Append($"var stream = await _service.{rpcName}({(request != PrimitiveType.Void ? "__request" : "")});").AppendLine();
                    methodWriter.Append($"return new FileStreamResult(stream, \"{contentType}\") {{ FileDownloadName = {fileName} }};");
                }
                else
                {
                    methodWriter.Append($"return _service.{rpcName}({(request != PrimitiveType.Void ? "__request" : "")});");
                }
            }

            clsWriter.Append("}").AppendLine();
            writer.Save($"{srvInfo.Name}Controller");
        }

        private static void AddRequestMethod(CodeWriter bodyWriter, RpcOptions options, string template)
        {
            switch (options.Method)
            {
                case "GET":
                    bodyWriter.Append($"[HttpGet{template}]");
                    break;
                case "POST":
                    bodyWriter.Append($"[HttpPost{template}]");
                    break;
                case "PUT":
                    bodyWriter.Append($"[HttpPut{template}]");
                    break;
                case "DELETE":
                    bodyWriter.Append($"[HttpDelete{template}]");
                    break;
            }
        }

        private static void AddAutorizationAttribute(SecurityOptions option, CodeWriter clsWriter)
        {
            if (option.Authorized)
            {
                clsWriter.Append("[Authorize]").AppendLine();
                
            }
            else if (option.Roles != null)
            {
                clsWriter.Append($"[Authorize(Roles = \"{option.Roles}\")]").AppendLine();                
            }
            else if (option.Policy != null)
            {
                clsWriter.Append($"[Authorize(Policy = \"{option.Policy}\")]").AppendLine();                
            }
            else if(option.AllowAnonymous)
            {
                clsWriter.Append("[AllowAnonymous]").AppendLine();
            }
        }

        private object GetRequestBinding(string method, ITypeDeclaration request)
        {
            if (request == PrimitiveType.Void)
                return "";

            switch (method)
            {
                case "DELETE":
                case "GET":
                    return "[FromQuery]";
                case "PATCH":
                case "PUT":
                case "POST":
                    return request.HasStreams() ? "[ModelBinder(typeof(CybtansModelBinder))]" : "[FromBody]";
                default:
                    throw new NotImplementedException("Http verb is not valid or not supported");
            }
        }
            
    }
}
