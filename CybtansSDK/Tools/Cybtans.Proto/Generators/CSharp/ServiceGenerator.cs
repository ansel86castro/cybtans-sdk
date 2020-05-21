using Cybtans.Proto.AST;
using Cybtans.Proto.Utils;
using System;
using System.Collections.Generic;
using System.IO;

namespace Cybtans.Proto.Generators.CSharp
{
    public class ServiceGenerator : FileGenerator
    {
        private TypeGenerator _typeGenerator;

        public ServiceGenerator(ProtoFile proto, TypeGeneratorOption option, TypeGenerator typeGenerator) :base(proto, option)
        {
            this._typeGenerator = typeGenerator;
            Namespace = $"{proto.Option.Namespace}.{option.Namespace ?? "Services"}";

            foreach (var item in _proto.Declarations)
            {
                if (item is ServiceDeclaration srv)
                {
                    var info = new ServiceGenInfo(srv, _option, _proto);             

                    Services.Add(srv, info);
                }
            }
        }

        public string Namespace { get; }

        public Dictionary<ServiceDeclaration, ServiceGenInfo> Services { get; } = new Dictionary<ServiceDeclaration, ServiceGenInfo>();

        public override void GenerateCode()
        {
            Directory.CreateDirectory(_option.OutputDirectory);

            foreach (var item in Services)
            {               
                GenerateService(item.Value);                    
            }
        }

        private void GenerateService(ServiceGenInfo info)
        {
            var writer = CreateWriter(info.Namespace);
           
            writer.Usings.Append("using System.Threading.Tasks;").AppendLine();
            writer.Usings.Append($"using {_typeGenerator.Namespace};").AppendLine();
            writer.Usings.Append("using System.Collections.Generic;").AppendLine();

            var clsWriter = writer.Class;

            clsWriter.Append("public abstract");

            if (_option.PartialClass)
            {
                clsWriter.Append(" partial");
            }

            clsWriter.Append($" class {info.Name} ").AppendLine();                 
                        
            clsWriter.Append("{").AppendLine();
            clsWriter.Append('\t', 1);

            var bodyWriter = clsWriter.Block("BODY");

            foreach (var rpc in info.Service.Rpcs)
            {
                var returnInfo = rpc.ResponseType;
                var requestInfo = rpc.RequestType;               

                bodyWriter.AppendLine();
                bodyWriter.Append($"public abstract {returnInfo.GetReturnTypeName()} { GetRpcName(rpc)}({requestInfo.GetRequestTypeName("request")});");
                bodyWriter.AppendLine();
                bodyWriter.AppendLine();
            }

            clsWriter.Append("}").AppendLine();

            writer.Save(info.Name);
        }

        public string GetRpcName(RpcDeclaration rpc)
        {
            return rpc.Name.Pascal();
        }  
        
      
    }
}
