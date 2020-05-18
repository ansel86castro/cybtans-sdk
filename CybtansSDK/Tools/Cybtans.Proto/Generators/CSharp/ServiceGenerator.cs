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
           
            writer.Usings.Append($"using System.Threading.Tasks;").AppendLine();

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

            for (int i = 0; i < info.Service.Rpcs.Count; i++)
            {
                var rpc = info.Service.Rpcs[i];

                var returnInfo = _typeGenerator.Messages[rpc.ResponseType];
                var requestInfo = _typeGenerator.Messages[rpc.RequestType];

                if (i == 0)
                {
                    writer.Usings.Append("using ").Append(requestInfo.Namespace).Append(";").AppendLine();
                }

                bodyWriter.AppendLine();
                bodyWriter.Append($"public abstract Task<{returnInfo.Name}> {GetRpcName(rpc)}({requestInfo.Name} request");
                if(info.Service.Option.ContextClass != null)
                {
                    bodyWriter.Append($", global::{info.Service.Option.ContextClass} context");
                }
                
                bodyWriter.Append(");");

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
