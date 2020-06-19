using System;
using System.Collections.Generic;
using System.Reflection;

namespace Cybtans.Services.DependencyInjection
{
    public class AssemblyTypeResolver : ITypeResolver
    {
        struct RegisterInfo
        {
            public Type Type;

            public LifeType LifeType;
        }

        Dictionary<Type, RegisterInfo> mappings = new Dictionary<Type, RegisterInfo>();
        bool isInitialized;

        public AssemblyTypeResolver(IEnumerable<Assembly> assemblies)
        {
            Assemblies.AddRange(assemblies);
        }

        public List<Assembly> Assemblies { get; private set; } = new List<Assembly>();

        public void Init()
        {
            mappings.Clear();
            foreach (var assembly in Assemblies)
            {
                var types = assembly.GetExportedTypes();
                foreach (var type in types)
                {
                    var attr = type.GetCustomAttribute<RegisterDependencyAttribute>();
                    if (attr != null)
                    {
                        if (attr.Contract != null)
                        {
                            mappings[attr.Contract] = new RegisterInfo
                            {
                                Type = type,
                                LifeType = attr.LifeType
                            };
                        }

                        if (attr.Contracts != null)
                        {
                            foreach (var c in attr.Contracts)
                            {
                                mappings[c] = new RegisterInfo
                                {
                                    Type = type,
                                    LifeType = attr.LifeType
                                };
                            }
                        }
                    }
                }
            }

            isInitialized = true;
        }

        public Type ResolveImplementer(Type contract, out LifeType? lifeType)
        {
            if (!isInitialized)
                Init();

            if (!mappings.TryGetValue(contract, out RegisterInfo info) && contract.IsGenericType)
            {
                var genDef = contract.GetGenericTypeDefinition();
                if (mappings.TryGetValue(genDef, out info))
                {
                    info.Type = info.Type.MakeGenericType(contract.GetGenericArguments());
                }
            }

            lifeType = info.LifeType;
            return info.Type;
        }
    }


}
