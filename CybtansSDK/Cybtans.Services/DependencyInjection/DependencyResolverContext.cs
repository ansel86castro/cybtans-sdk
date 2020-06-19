using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Cybtans.Services.DependencyInjection
{
    public class DependencyResolverContext : IServiceProvider, IDisposable
    {
        struct TypeResolveInfo
        {
            public object Singleton;

            public IFactory Factory;

            public LifeType LifeType;
        }


        private static DependencyResolverContext instance;

        List<ITypeResolver> typeResolvers = new List<ITypeResolver>();

        Dictionary<Type, TypeResolveInfo> typeInfoMap = new Dictionary<Type, TypeResolveInfo>();

        Dictionary<Type, List<string>> propertyInfoMap = new Dictionary<Type, List<string>>();

        public DependencyResolverContext()
        {

        }

        public DependencyResolverContext(IEnumerable<Assembly> assemblies)
        {
            typeResolvers.Add(new AssemblyTypeResolver(assemblies));
        }

        public List<ITypeResolver> TypeResolvers => typeResolvers;

        public static DependencyResolverContext Instance { get { return instance; } set { instance = value; } }

        public LifeType? GetLifeType(Type type)
        {
            if (typeInfoMap.TryGetValue(type, out TypeResolveInfo info))
            {
                return info.LifeType;
            }

            return null;
        }

        public DependencyResolverScope CreateScope()
        {
            return new DependencyResolverScope(this);
        }

        public DependencyResolverContext Register<T>(object singleton)
        {
            typeInfoMap[typeof(T)] = new TypeResolveInfo
            {
                LifeType = LifeType.Singleton,
                Singleton = singleton
            };

            return this;
        }

        public DependencyResolverContext Register<T>(Func<T> function, LifeType lifeType)
        {
            return Register(typeof(T), new DelegateFactory<T>(function), lifeType);
        }

        public DependencyResolverContext Register<T>(Func<IServiceProvider, T> function, LifeType lifeType)
        {
            return Register(typeof(T), new DelegateFactory2<T>(function), lifeType);
        }

        public DependencyResolverContext Register<T, I>(LifeType life)
        {
            return Register(typeof(T), new ActivatorFactory(typeof(I)), life);
        }

        public DependencyResolverContext Register(Type service, Type implementation, LifeType life)
        {
            return Register(service, new ActivatorFactory(implementation), life);
        }

        public DependencyResolverContext Register(Type type, IFactory factory, LifeType life)
        {
            typeInfoMap[type] = new TypeResolveInfo
            {
                Factory = factory,
                LifeType = life,
            };

            return this;
        }

        public DependencyResolverContext RegisterInjectableProperty<T>(string property)
        {
            return RegisterInjectableProperty(typeof(T), property);
        }

        public DependencyResolverContext RegisterInjectableProperty<T>(Expression<Func<T, object>> memberExp)
        {
            var expression = memberExp.Body as MemberExpression;
            return RegisterInjectableProperty(typeof(T), expression.Member.Name);
        }

        public DependencyResolverContext RegisterInjectableProperty(Type type, string property)
        {
            if (!propertyInfoMap.TryGetValue(type, out List<string> propsList))
            {
                propsList = new List<string>();
                propertyInfoMap.Add(type, propsList);
            }
            propsList.Add(property);
            return this;
        }

        public object GetService(Type serviceType)
        {
            return GetService(serviceType, null);
        }

        public T GetService<T>()
        {
            return (T)GetService(typeof(T));
        }

        internal object GetService(Type serviceType, DependencyResolverScope scope)
        {
            object service = null;
            Type implementer = null;
            LifeType? life = null;
            bool mapped;
            if (mapped = typeInfoMap.TryGetValue(serviceType, out TypeResolveInfo info) ||
                serviceType.IsGenericType && typeInfoMap.TryGetValue(serviceType.GetGenericTypeDefinition(), out info))
            {
                life = info.LifeType;

                if (info.Singleton != null)
                    return info.Singleton;

                if (scope != null && info.LifeType == LifeType.Scope)
                {
                    service = scope.GetFromCache(serviceType) ?? info.Factory?.CreateInstance((IServiceProvider)scope ?? this, serviceType);
                }
                else
                {
                    service = info.Factory?.CreateInstance((IServiceProvider)scope ?? this, serviceType);
                }

            }
            else
            {
                if (!serviceType.IsInterface && !serviceType.IsAbstract)
                {
                    implementer = serviceType;
                    life = LifeType.Default;
                }
                else
                {
                    foreach (var resolver in TypeResolvers)
                    {
                        implementer = resolver.ResolveImplementer(serviceType, out life);
                        if (implementer != null)
                            break;
                    }
                }

                if (implementer != null)
                {
                    service = CreateInstance(implementer, scope);

                }
            }

            if (service != null && life != null)
            {
                switch (life)
                {
                    case LifeType.Scope:
                        scope?.AddToCache(serviceType, service);
                        break;
                    case LifeType.Singleton:
                        info.Singleton = service;
                        typeInfoMap[serviceType] = info;
                        break;
                    default:
                        break;
                }
            }

            return service;

        }

        private object GetScopedOrGlobalService(DependencyResolverScope scope, Type type)
        {
            return scope != null ?
                scope.GetService(type) :
                GetService(type);
        }

        private object CreateInstance(Type mappedType, DependencyResolverScope scope)
        {
            var constructor = mappedType.GetConstructors(BindingFlags.Public | BindingFlags.Instance);
            object instance;
            if (constructor.Length == 0)
            {
                instance = Activator.CreateInstance(mappedType);
            }
            else
            {
                var parameters = constructor[0].GetParameters();
                object[] paramInstances = new object[parameters.Length];
                for (int i = 0; i < parameters.Length; i++)
                {
                    paramInstances[i] = GetScopedOrGlobalService(scope, parameters[i].ParameterType);
                }
                instance = constructor[0].Invoke(paramInstances);
            }

            return instance;
        }

        public void Dispose()
        {
            foreach (var item in typeInfoMap.Values)
            {
                if (item.Singleton is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
        }
    }
}
