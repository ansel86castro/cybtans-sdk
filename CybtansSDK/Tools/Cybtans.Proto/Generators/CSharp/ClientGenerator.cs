using Cybtans.Proto.AST;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace Cybtans.Proto.Generators.CSharp
{
    public class ClientGenerator : FileGenerator
    {
        private readonly ServiceGenerator _serviceGenerator;
        private readonly TypeGenerator _typeGenerator;

        public string Namespace { get; }

        public ClientGenerator(ProtoFile proto, TypeGeneratorOption option, ServiceGenerator serviceGenerator, TypeGenerator typeGenerator) 
            : base(proto, option)
        {
            _serviceGenerator = serviceGenerator;
            _typeGenerator = typeGenerator;

            Namespace = $"{proto.Option.Namespace}.{option.Namespace ?? "Clients"}";           
        }

        public override void GenerateCode()
        {
            Directory.CreateDirectory(_option.OutputDirectory);

            foreach (var item in _serviceGenerator.Services)
            {
                GenerateClient(item.Value);

                GenerateExtensions(item.Value);
            }
        }

        private void GenerateClient(ServiceGenInfo info)
        {
            var writer = CreateWriter(Namespace);

            writer.Usings.Append($"using Refit;").AppendLine();
            writer.Usings.Append($"using System.Net.Http;").AppendLine();
            writer.Usings.Append($"using System.Threading.Tasks;").AppendLine();
            writer.Usings.Append($"using {_typeGenerator.Namespace};").AppendLine();

            var clsWriter = writer.Class;
            clsWriter.Append($"public interface I{info.Name}").AppendLine();
            clsWriter.Append("{").AppendLine();
            clsWriter.Append('\t', 1);

            var bodyWriter = clsWriter.Block("BODY");

            var srv = info.Service;

            foreach (var rpc in srv.Rpcs)
            {
                var options = rpc.Option;
                var request = rpc.RequestType;
                var response = rpc.ResponseType;
                var rpcName = _serviceGenerator.GetRpcName(rpc);
                string url = $"/{srv.Option.Prefix}";
                List<MessageFieldInfo>? path = null;
                if(options.Template != null)
                {
                    var template = options.Template;
                    path = request is MessageDeclaration ? _typeGenerator.Messages[request].GetPathBinding(template) : null;
                    if(path != null)
                    {
                        foreach (var field in path)
                        {
                            template = template.Replace($"{{{field.Field.Name}}}", $"{{request.{field.Name}}}");
                        }
                    }
                    url =$"/{srv.Option.Prefix}/{ template}";
                }                                

                bodyWriter.AppendLine();

                if (srv.Option.RequiredAuthorization || options.RequiredAuthorization)
                {
                    bodyWriter.Append("[Headers(\"Authorization: Bearer\")]").AppendLine();
                }

                string optional = "";

                switch (options.Method)
                {
                    case "GET":
                        bodyWriter.Append($"[Get(\"{url}\")]");
                        if((path == null || path.Count == 0) && request != PrimitiveType.Void)
                        {
                            optional = " = null";
                        }
                        break;
                    case "POST":
                        bodyWriter.Append($"[Post(\"{url}\")]");
                        break;
                    case "PUT":
                        bodyWriter.Append($"[Put(\"{url}\")]");
                        break;
                    case "DELETE":
                        bodyWriter.Append($"[Delete(\"{url}\")]");
                        break;
                }

                bodyWriter.AppendLine();

                bodyWriter.Append($"{response.GetReturnTypeName()} {rpcName}({GetRequestBinding(options.Method)}{request.GetRequestTypeName("request")}{optional});").AppendLine();
            }

            clsWriter.AppendLine().Append("}").AppendLine();

            writer.Save($"I{info.Name}");

        }       

        private void GenerateExtensions(ServiceGenInfo info)
        {
            var writer = CreateWriter(Namespace);
           
            writer.Usings.Append("using Refit;").AppendLine();
            writer.Usings.Append("using Cybtans.Refit;").AppendLine();
            writer.Usings.Append("using Cybtans.Serialization;").AppendLine(); 
            writer.Usings.Append("using Microsoft.Extensions.Configuration;").AppendLine();
            writer.Usings.Append("using Microsoft.Extensions.DependencyInjection;").AppendLine();
            writer.Usings.Append("using System.Text;")    
                .AppendLine(2);

            writer.Class.AppendTemplate(setupExtension, new Dictionary<string, object>
            {
                ["NAME"] = info.Name
            });

            writer.Save($"{info.Name}Extensions");
        }

        private object GetRequestBinding(string method)
        {
            return method switch
            {
                "GET" => "",
                "POST" => "[Body(buffered: true)]",
                "PUT" => "[Body(buffered: true)]",
                "DELETE" => "",
                _ => throw new NotImplementedException()
            };
        }

        string setupExtension = @"
public class @{NAME}Option
{
    public string BaseUrl { get; set; }
}

public static class @{NAME}Extensions
{
    public static void Add@{NAME}(this IServiceCollection services, IConfiguration configuration, RefitSettings settings = null)
    {
        var option = configuration.GetSection(""@{NAME}"").Get<@{NAME}Option>();

        if(settings == null)
        {
            settings = new RefitSettings();
        }

        settings.ContentSerializer = new CybtansContentSerializer(settings.ContentSerializer);

        var builder = services.AddRefitClient<I@{NAME}>(settings);

        builder.ConfigureHttpClient(c =>
        {                
            c.BaseAddress = new Uri(option.BaseUrl);
            c.DefaultRequestHeaders.Add(""Accept"", $""{BinarySerializer.MEDIA_TYPE}; charset={Encoding.UTF8.WebName}"");
        });
    }       
}
";
    }
}
