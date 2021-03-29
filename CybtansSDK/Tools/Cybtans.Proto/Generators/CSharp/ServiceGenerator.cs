using Cybtans.Proto.AST;
using Cybtans.Proto.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;

namespace Cybtans.Proto.Generators.CSharp
{
    public class ServiceGenerator : FileGenerator<ServiceGeneratorOptions>
    {
        private TypeGenerator _typeGenerator;

        public ServiceGenerator(ProtoFile proto, ServiceGeneratorOptions option, TypeGenerator typeGenerator) :base(proto, option)
        {
            this._typeGenerator = typeGenerator;
            Namespace = option.Namespace ?? $"{proto.Option.Namespace ?? proto.Filename.Pascal()}.Services";

            foreach (var item in Proto.Declarations)
            {
                if (item is ServiceDeclaration srv)
                {
                    var info = new ServiceGenInfo(srv, _option, Proto);             

                    Services.Add(srv, info);
                }
            }
        }

        public string Namespace { get; }

        public Dictionary<ServiceDeclaration, ServiceGenInfo> Services { get; } = new Dictionary<ServiceDeclaration, ServiceGenInfo>();

        public override void GenerateCode()
        {
            Directory.CreateDirectory(_option.OutputPath);
            
            foreach (var item in Services)
            {
                var srv = item.Value;
                GenerateService(srv);

                if (srv.Service.Option.GrpcProxy)
                {                    
                    GenerateGrpsProxy(srv);
                }
            }        
        }     
        
        public string GetInterfaceName(ServiceDeclaration service)
        {
            if (_option.NameTemplate != null)
            {
                return $"I{TemplateProcessor.Process(_option.NameTemplate, new { Name = service.Name.Pascal() })}";
            }

            return $"I{service.Name.Pascal()}";
        }

        public string GetImplementationName(ServiceDeclaration service)
        {
            if (_option.NameTemplate != null)
            {
                return TemplateProcessor.Process(_option.NameTemplate, new { Name = service.Name.Pascal() });
            }

            return service.Name.Pascal();
        }


        private void GenerateService(ServiceGenInfo info)
        {
            var writer = CreateWriter(info.Namespace);
           
            writer.Usings.Append("using System.Threading.Tasks;").AppendLine();
            writer.Usings.Append($"using {_typeGenerator.Namespace};").AppendLine();
            writer.Usings.Append("using System.Collections.Generic;").AppendLine();

            var clsWriter = writer.Class;

            if (info.Service.Option.Description != null)
            {
                clsWriter.Append("/// <summary>").AppendLine();
                clsWriter.Append("/// ").Append(info.Service.Option.Description).AppendLine();
                clsWriter.Append("/// </summary>").AppendLine();                
            }

            clsWriter.Append("public");

            if (_option.PartialClass)
            {
                clsWriter.Append(" partial");
            }

            var typeName = GetInterfaceName(info.Service);

            clsWriter.Append($" interface {typeName} ").AppendLine();                 
                        
            clsWriter.Append("{").AppendLine();
            clsWriter.Append('\t', 1);

            var bodyWriter = clsWriter.Block("BODY");

            foreach (var rpc in info.Service.Rpcs)
            {
                var returnInfo = rpc.ResponseType;
                var requestInfo = rpc.RequestType;               

                bodyWriter.AppendLine();
                if (rpc.Option.Description != null)
                {
                    bodyWriter.Append("/// <summary>").AppendLine();
                    bodyWriter.Append("/// ").Append(rpc.Option.Description).AppendLine();
                    bodyWriter.Append("/// </summary>").AppendLine();
                }

                bodyWriter.Append($"{returnInfo.GetReturnTypeName()} { GetRpcName(rpc)}({requestInfo.GetRequestTypeName("request")});");
                bodyWriter.AppendLine();
                bodyWriter.AppendLine();
            }

            clsWriter.Append("}").AppendLine();

            writer.Save(typeName);
        }

        public string GetRpcName(RpcDeclaration rpc)
        {
            return rpc.Name.Pascal();
        }  
        
      
        private void GenerateGrpsProxy(ServiceGenInfo info)
        {
            var writer = CreateWriter(info.Namespace);
         
            writer.Usings.Append("using System.Threading.Tasks;").AppendLine();            
            writer.Usings.Append("using System.Collections.Generic;").AppendLine();       
            writer.Usings.Append("using Grpc.Core;").AppendLine(); 
            writer.Usings.Append($"using {_typeGenerator.Namespace};").AppendLine();

            var proxyName = GetImplementationName(info.Service);
            var interfaceName = GetInterfaceName(info.Service);


            var clsWriter = writer.Class;

            if (_option.AutoRegisterImplementation)
            {
                writer.Usings.Append("using Cybtans.Services;").AppendLine();
                clsWriter.Append($"[RegisterDependency(typeof({ interfaceName}))]").AppendLine();
            }
            
            clsWriter.Append("public");

            if (_option.PartialClass)
            {
                clsWriter.Append(" partial");
            }            
          
            clsWriter.Append($" class {proxyName} : {interfaceName}").AppendLine();

            clsWriter.Append("{").AppendLine();
            clsWriter.Append('\t', 1);

            var bodyWriter = clsWriter.Block("BODY");

            var grpcClientType = $"{Proto.Option.Namespace}.{info.Name}.{info.Name}Client";

            bodyWriter.Append($"private readonly {grpcClientType}  _client;").AppendLine()                   
                      .Append($"private readonly ILogger<{proxyName}> _logger;").AppendLine()
                      .AppendLine();

            #region Constructor

            bodyWriter.Append($"public {proxyName}({grpcClientType} client, ILogger<{proxyName}> logger)").AppendLine();
            bodyWriter.Append("{").AppendLine();
            bodyWriter.Append('\t', 1).Append("_client = client;").AppendLine();         
            bodyWriter.Append('\t', 1).Append("_logger = logger;").AppendLine();
            bodyWriter.Append("}").AppendLine();

            #endregion

            foreach (var rpc in info.Service.Rpcs)
            {
                var returnInfo = rpc.ResponseType;
                var requestInfo = rpc.RequestType;
                var rpcName = GetRpcName(rpc);             

                var requestTypeName = requestInfo.GetTypeName();
                var returnTypeName = returnInfo.GetTypeName();             

                bodyWriter.AppendLine();
              
                bodyWriter.Append($"public async {returnInfo.GetReturnTypeName()} { GetRpcName(rpc)}({requestInfo.GetRequestTypeName("request")})").AppendLine();
                bodyWriter.Append("{").AppendLine().Append('\t', 1);

                var methodWriter = bodyWriter.Block($"METHODBODY_{rpc.Name}");

                methodWriter.Append("try").AppendLine()
                    .Append("{").AppendLine();
              
                methodWriter.Append('\t', 1).Append($"var response = await _client.{rpcName}Async({(!PrimitiveType.Void.Equals(requestInfo) ? $"request.ToProtobufModel()" : "")});").AppendLine();
                methodWriter.Append('\t', 1).Append($"return response.ToPocoModel();").AppendLine();

                methodWriter.Append("}").AppendLine();
                methodWriter.Append("catch(RpcException ex)").AppendLine()
                    .Append("{").AppendLine();

                methodWriter.Append('\t', 1).Append($"_logger.LogError(ex, \"Failed grpc call {grpcClientType}.{rpc.Name}\");").AppendLine();
                methodWriter.Append('\t', 1).Append("throw;").AppendLine();

                methodWriter.Append("}").AppendLine();

                bodyWriter.Append("}").AppendLine();
            }

            clsWriter.Append("}").AppendLine();

            writer.Save(proxyName);
        }

      
    }
}
