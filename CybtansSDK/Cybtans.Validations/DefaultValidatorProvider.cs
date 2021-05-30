using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cybtans.Validations
{
    public class DefaultValidatorProvider : IValidatorProvider
    {
        private readonly Dictionary<Type, IValidator> _validators = new Dictionary<Type, IValidator>();

        public DefaultValidatorProvider()
        {

        }

        public void AddValidators(params IValidator[] validators)
        {
            foreach (var item in validators)
            {
                var type = item.GetType();
                var baseType = type.BaseType;
                if (baseType.IsGenericType)
                {
                    var genArgs = baseType.GetGenericArguments();
                    _validators.Add(genArgs[0], item);
                }
                else
                {
                    throw new InvalidOperationException("validator not supported");
                }
            }
        }

        public void AddValidatorFromAssembly(params Assembly[] assemblies)
        {
            foreach (var type in assemblies.SelectMany(x => x.ExportedTypes))
            {
                if (typeof(IValidator).IsAssignableFrom(type))
                {
                    var baseType = type.BaseType;
                    if (baseType.IsGenericType)
                    {
                        var genArgs = baseType.GetGenericArguments();
                        _validators.Add(genArgs[0], (IValidator)Activator.CreateInstance(type));
                    }
                }
            }
        }

        public bool TryGetValidator<TRequest>(out IValidator<TRequest> result) where TRequest : class
        {
            result = null;
            if (_validators.TryGetValue(typeof(TRequest), out var validator))
            {
                result = validator as IValidator<TRequest>;
                return result != null;
            }

            return false;
        }
    }
}
