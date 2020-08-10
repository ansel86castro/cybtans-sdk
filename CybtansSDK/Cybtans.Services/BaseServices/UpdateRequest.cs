using System;
using Cybtans.Serialization;

namespace Cybtans.Services
{
    internal class UpdateRequest<T, TKey> : IReflectorMetadataProvider
    {
        private static readonly UpdateBuildingRequestAccesor __accesor = new UpdateBuildingRequestAccesor();

        public TKey Id { get; set; }

        public T Value { get; set; }

        public IReflectorMetadata GetAccesor()
        {
            return __accesor;
        }

        public sealed class UpdateBuildingRequestAccesor : IReflectorMetadata
        {
            public const int Id = 1;
            public const int Value = 2;
            private readonly int[] _props = new[]
            {
            Id,Value
        };

            public int[] GetPropertyCodes() => _props;

            public string GetPropertyName(int propertyCode)
            {
                return propertyCode switch
                {
                    Id => "Id",
                    Value => "Value",

                    _ => throw new InvalidOperationException("property code not supported"),
                };
            }

            public int GetPropertyCode(string propertyName)
            {
                return propertyName switch
                {
                    "Id" => Id,
                    "Value" => Value,

                    _ => -1,
                };
            }

            public Type GetPropertyType(int propertyCode)
            {
                return propertyCode switch
                {
                    Id => typeof(TKey),
                    Value => typeof(T),

                    _ => throw new InvalidOperationException("property code not supported"),
                };
            }

            public object GetValue(object target, int propertyCode)
            {
                UpdateRequest<T, TKey> obj = (UpdateRequest<T, TKey>)target;
                return propertyCode switch
                {
                    Id => obj.Id,
                    Value => obj.Value,

                    _ => throw new InvalidOperationException("property code not supported"),
                };
            }

            public void SetValue(object target, int propertyCode, object value)
            {
                UpdateRequest<T, TKey> obj = (UpdateRequest<T, TKey>)target;
                switch (propertyCode)
                {
                    case Id: obj.Id = (TKey)value; break;
                    case Value: obj.Value = (T)value; break;

                    default: throw new InvalidOperationException("property code not supported");
                }
            }

        }
    }
}
