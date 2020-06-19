using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cybtans.Services.DependencyInjection
{
    public interface IFactory
    {
        object CreateInstance(IServiceProvider serviceProvider, Type requestType);
    }

    public class DelegateFactory<T> : IFactory
    {
        Func<T> function;

        public DelegateFactory(Func<T> function)
        {
            this.function = function;
        }

        public object CreateInstance(IServiceProvider serviceProvider, Type requestType)
        {
            return function();
        }
    }

    public class DelegateFactory2<T> : IFactory
    {
        Func<IServiceProvider, T> function;

        public DelegateFactory2(Func<IServiceProvider, T> function)
        {
            this.function = function;
        }

        public object CreateInstance(IServiceProvider serviceProvider, Type requestType)
        {
            return function(serviceProvider);
        }
    }


    public class ActivatorFactory : IFactory
    {
        Type mappedType;

        public ActivatorFactory(Type mappedType)
        {
            this.mappedType = mappedType;
        }

        public object CreateInstance(IServiceProvider serviceProvider, Type requestType)
        {
            Type servType = mappedType;
            if (mappedType.IsGenericTypeDefinition)
            {
                var genArgs = requestType.GetGenericArguments();
                servType = mappedType.MakeGenericType(genArgs);
            }


            var constructor = servType.GetConstructors(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            object instance;
            if (constructor.Length == 0)
            {
                instance = Activator.CreateInstance(servType);
            }
            else
            {
                var parameters = constructor[0].GetParameters();
                object[] paramInstances = new object[parameters.Length];
                for (int i = 0; i < parameters.Length; i++)
                {
                    paramInstances[i] = serviceProvider.GetService(parameters[i].ParameterType);
                }
                instance = constructor[0].Invoke(paramInstances);
            }

            return instance;
        }
    }
}
