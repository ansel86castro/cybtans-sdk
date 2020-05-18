using Cybtans.Proto.AST;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Cybtans.Proto.Generators.CSharp
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
            typeGenerator.GenerateCode();

            if (_options.ServiceOptions != null)
            {
                var serviceGenerator = new ServiceGenerator(proto, _options.ServiceOptions, typeGenerator);
                serviceGenerator.GenerateCode();

                if (_options.ControllerOptions != null)
                {
                    var controlller = new WebApiControllerGenerator(proto, _options.ControllerOptions, serviceGenerator, typeGenerator);
                    controlller.GenerateCode();

                    if(_options.ClientOptions != null)
                    {
                        var client = new ClientGenerator(proto, _options.ClientOptions, serviceGenerator, typeGenerator);
                        client.GenerateCode();
                    }
                }
            }                        
        }     
    }
}
