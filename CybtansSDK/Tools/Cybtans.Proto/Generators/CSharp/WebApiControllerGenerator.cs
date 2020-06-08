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
    public class WebApiControllerGenerator : FileGenerator
    {
        ServiceGenerator _serviceGenerator;
        TypeGenerator _typeGenerator;

        public WebApiControllerGenerator(ProtoFile proto, TypeGeneratorOption option,
         ServiceGenerator serviceGenerator, TypeGenerator typeGenerator) : base(proto, option)
        {
            _serviceGenerator = serviceGenerator;
            _typeGenerator = typeGenerator;
        }

        public override void GenerateCode()
        {
            Directory.CreateDirectory(_option.OutputDirectory);

            foreach (var item in _serviceGenerator.Services)
            {              
                var srvInfo = item.Value;

                GenerateController(srvInfo);
            }
        }

        private void GenerateController(ServiceGenInfo srvInfo)
        {
            var srv = srvInfo.Service;
            var writer = CreateWriter($"{_proto.Option.Namespace}.{_option.Namespace ?? "Controllers"}");

            writer.Usings.Append($"using System.Collections.Generic;").AppendLine();
            writer.Usings.Append($"using System.Threading.Tasks;").AppendLine();

            writer.Usings.Append($"using Microsoft.AspNetCore.Http;").AppendLine();
            writer.Usings.Append($"using Microsoft.AspNetCore.Mvc;").AppendLine();


            writer.Usings.Append($"using {_serviceGenerator.Namespace};").AppendLine();
            writer.Usings.Append($"using {_typeGenerator.Namespace};").AppendLine();

            var clsWriter = writer.Class;

            if(srv.Option.RequiredAuthorization || srv.Option.AllowAnonymous || 
               srv.Rpcs.Any(x=>x.Option.RequiredAuthorization || x.Option.AllowAnonymous))
            {
                writer.Usings.Append("using Microsoft.AspNetCore.Authorization;").AppendLine();
            }

            AddAutorizationAttribute(srv.Option, clsWriter);

            clsWriter.Append($"[Route(\"{srv.Option.Prefix}\")]").AppendLine();
            clsWriter.Append("[ApiController]").AppendLine();
            clsWriter.Append($"public class {srvInfo.Name}Controller : ControllerBase").AppendLine();

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

                bodyWriter.AppendLine();
                bodyWriter.Append($"public async {response.GetReturnTypeName()} {rpcName}").Append("(");
                var parametersWriter = bodyWriter.Block($"PARAMS_{rpc.Name}");
                bodyWriter.Append($"{GetRequestBinding(options.Method, request)}{request.GetRequestTypeName("__request")})").AppendLine()
                    .Append("{").AppendLine()
                    .Append('\t', 1);

                var methodWriter = bodyWriter.Block($"METHODBODY_{rpc.Name}");

                bodyWriter.AppendLine().Append("}").AppendLine();

                if (options.Template != null)
                {
                    var path = request is MessageDeclaration ? _typeGenerator.Messages[request].GetPathBinding(options.Template) : null;
                    if (path != null)
                    {
                        foreach (var field in path)
                        {
                            parametersWriter.Append($"{field.Type} {field.Field.Name}, ");
                            methodWriter.Append($"__request.{field.Name} = {field.Field.Name};").AppendLine();
                        }
                    }
                }

                if (response != PrimitiveType.Void)
                {
                    methodWriter.Append("return ");
                }

                methodWriter.Append($"await _service.{rpcName}");

                if (request != PrimitiveType.Void)
                {
                    methodWriter.Append("(__request);");
                }
                else
                {
                    methodWriter.Append("();");
                }
            }

            clsWriter.Append("}").AppendLine();
            writer.Save($"{srvInfo.Name}Controller");
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

            return method switch
            {
                "GET" => "[FromQuery]",
                "POST" => "[FromBody]",
                "PUT" => "[FromBody]",
                "DELETE" => "[FromQuery]",
                _ => throw new NotImplementedException()
            };
        }
            
    }
}
