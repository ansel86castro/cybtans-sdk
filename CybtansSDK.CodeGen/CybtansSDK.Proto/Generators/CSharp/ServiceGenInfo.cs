using CybtansSdk.Proto.AST;
using CybtansSdk.Proto.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace CybtansSdk.Proto.Generators.CSharp
{
    public class ServiceGenInfo
    {
        public ServiceDeclaration Service { get; }
        
        public string Name { get; }

        public string Namespace { get; }

        public ServiceGenInfo(ServiceDeclaration service, OutputOption outputOption, ProtoFile proto)
        {
            Service = service;

            Name = service.Name.Pascal();
            Namespace = $"{proto.Option.Namespace}.{outputOption.Namespace ?? "Services"}";

        }


    }   
}
