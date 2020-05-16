using CybtansSdk.Proto.AST;
using CybtansSdk.Proto.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CybtansSdk.Proto.Generators.CSharp
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

            writer.Usings.Append($"using System;").AppendLine();
            writer.Usings.Append($"using System.Collections.Generic;").AppendLine();
            writer.Usings.Append($"using System.Threading.Tasks;").AppendLine();

            writer.Usings.Append($"using Microsoft.AspNetCore.Http;").AppendLine();
            writer.Usings.Append($"using Microsoft.AspNetCore.Mvc;").AppendLine();
           

            writer.Usings.Append($"using {_serviceGenerator.Namespace};").AppendLine();
            writer.Usings.Append($"using {_typeGenerator.Namespace};").AppendLine();
            writer.Usings.Append($"using Microsoft.AspNetCore.Http;").AppendLine();

            var clsWriter = writer.Class;
           
            clsWriter.Append($"[Route(\"{srv.Option.Prefix}\"]").AppendLine();
            clsWriter.Append("[ApiController]").AppendLine();         
            clsWriter.Append($"public class {srvInfo.Name}Controller : ControllerBase").AppendLine();

            clsWriter.Append("{").AppendLine();
            clsWriter.Append('\t', 1);

            var bodyWriter = clsWriter.Block("BODY");
         
            bodyWriter.Append($"private readonly {srvInfo.Name} _service;").AppendLine().AppendLine();

            #region Constructor

            bodyWriter.Append($"public {srvInfo.Name}Controller({srvInfo.Name} service)").AppendLine();
            bodyWriter.Append("{").AppendLine();
            bodyWriter.Append('\t',1).Append("_service = service;").AppendLine();
            bodyWriter.Append("}").AppendLine();

            #endregion

            foreach (var rpc in srv.Rpcs)
            {
                var options = rpc.Option;
                var request = _typeGenerator.Messages[rpc.RequestType];
                var response = _typeGenerator.Messages[rpc.ResponseType];
                var rpcName = _serviceGenerator.GetRpcName(rpc);
                string template = options.Template != null ? $"(\"{options.Template}\")" : "";

                bodyWriter.AppendLine();

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
                bodyWriter.Append($"public async Task<{response.Name}> {rpcName}").Append("(");
                var parametersWriter = bodyWriter.Block($"PARAMS_{rpc.Name}");
                bodyWriter.Append($"{GetRequestBinding(options.Method)}{response.Name} __request)").AppendLine()
                    .Append("{").AppendLine()
                    .Append('\t', 1);

                var methodWriter = bodyWriter.Block($"METHODBODY_{rpc.Name}");

                bodyWriter.AppendLine().Append("}").AppendLine();

                if (options.Template != null)
                {
                    var path = GetPathBinding(options.Template, request);
                    if (path != null)
                    {
                        foreach (var field in path)
                        {
                            parametersWriter.Append($"{field.Type} {field.Field.Name}, ");

                            methodWriter.Append($"__request.{field.Name} = {field.Field.Name};").AppendLine();
                        }
                    }
                }

                methodWriter.Append($"return await _service.{rpcName}(__request);");
            }

            clsWriter.Append("}").AppendLine();
            writer.Save($"{srvInfo.Name}Controller");
        }

        private object GetRequestBinding(string method)
        {
            return method switch
            {
                "GET" => "[FromQuery]",
                "POST" => "[FromBody]",
                "PUT" => "[FromBody]",
                "DELETE" => "[FromQuery]",
                _ => throw new NotImplementedException()
            };
        }
      
        public List<MessageFieldInfo> GetPathBinding(string template, MessageClassInfo msg)
        {
            var regex = new Regex(@"{(\w+)}");
            MatchCollection matches = regex.Matches(template);
            if (matches.Any(x=>x.Success))
            {
                List<MessageFieldInfo> fields = new List<MessageFieldInfo>();
                foreach (Match math in matches)
                {
                    if (math.Success)
                    {
                        var name = math.Groups[1].Value;
                        var field = msg.Fields[name];
                        fields.Add(field);
                    }
                }

                return fields;
            }
            return null;
        }
    }
}
