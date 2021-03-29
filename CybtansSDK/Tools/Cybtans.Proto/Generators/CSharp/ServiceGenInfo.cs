using Cybtans.Proto.AST;
using Cybtans.Proto.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cybtans.Proto.Generators.CSharp
{
    public class ServiceGenInfo
    {
        public ServiceDeclaration Service { get; }
        
        public string Name { get; }

        public string Namespace { get; }

        public ServiceGenInfo(ServiceDeclaration service, ServiceGeneratorOptions outputOption, ProtoFile proto)
        {
            Service = service;

            Name = service.Name.Pascal();
            Namespace = outputOption.Namespace ?? $"{proto.Option.Namespace}.Services";

        }


    }   
}
