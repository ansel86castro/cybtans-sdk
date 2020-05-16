using CybtansSdk.Proto.AST;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace CybtansSdk.Proto.Generators.CSharp
{


    public class MicroserviceGenerator : ICodeGenerator
    {
        private GenerationOptions _options;        

        public MicroserviceGenerator(GenerationOptions options)
        {
            _options = options;            
        }

        public void GenerateCode(ProtoFile proto)
        {            
            foreach (var item in proto.ImportedFiles)
            {
                GenerateCode(item);
            }

            var typeGenerator = new TypeGenerator(proto, _options.ModelOptions);
            var serviceGenerator = new ServiceGenerator(proto, _options.ServiceOptions, typeGenerator);
            var controlller = new WebApiControllerGenerator(proto, _options.ControllerOptions, serviceGenerator, typeGenerator);

            typeGenerator.GenerateCode();
            serviceGenerator.GenerateCode();
            controlller.GenerateCode();
        }     
    }
}
