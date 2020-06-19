using System;
using System.Collections.Generic;

namespace Cybtans.Services.DependencyInjection
{
    public class DependencyResolverScope : IServiceProvider, IDisposable
    {
        Dictionary<Type, object> scopedInstance = new Dictionary<Type, object>();
        DependencyResolverContext context;

        public DependencyResolverScope(DependencyResolverContext context)
        {
            this.context = context;
        }

        public void Dispose()
        {
            foreach (var item in scopedInstance.Values)
            {
                if (item is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }

            scopedInstance.Clear();
        }

        public object GetService(Type serviceType)
        {
            if (scopedInstance.TryGetValue(serviceType, out object instance))
            {
                return instance;
            }

            return context.GetService(serviceType, this);

        }

        internal void AddToCache(Type serviceType, object service)
        {
            if (!scopedInstance.ContainsKey(serviceType))
                scopedInstance.Add(serviceType, service);
        }

        internal object GetFromCache(Type serviceType)
        {
            scopedInstance.TryGetValue(serviceType, out object service);
            return service;
        }
    }

}
