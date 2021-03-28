using Cybtans.Proto.AST;
using Cybtans.Proto.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cybtans.Proto.Generators.CSharp
{
    public class ApiGatewayGenerator : WebApiControllerGenerator
    {
        private ClientGenerator _clientGenerator;

        public ApiGatewayGenerator(ProtoFile proto, 
            ApiGateWayGeneratorOption option, 
            ServiceGenerator serviceGenerator, 
            TypeGenerator typeGenerator,
            ClientGenerator clientGenerator)
            : base(proto, option, serviceGenerator, typeGenerator)
        {
            _clientGenerator = clientGenerator;
        }

        protected override void GenerateController(ServiceGenInfo srvInfo)
        {
            var writer = CreateWriter(_option.Namespace ?? $"{Proto.Option.Namespace ?? Proto.Filename.Pascal()}.Controllers");

            writer.Usings.Append($"using {_clientGenerator.Namespace};").AppendLine();
            writer.Usings.Append($"using {_typeGenerator.Namespace};").AppendLine();
            GenerateControllerInternal(srvInfo, writer);
        }
    }
}
