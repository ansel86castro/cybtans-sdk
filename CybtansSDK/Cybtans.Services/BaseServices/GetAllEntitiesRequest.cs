using System;
using Cybtans.Serialization;

namespace Cybtans.Services
{
    internal partial class GetAllEntitiesRequest : IReflectorMetadataProvider
    {
        private static readonly GetAllRequestAccesor __accesor = new GetAllRequestAccesor();

        public string Filter { get; set; }

        public string Sort { get; set; }

        public int? Skip { get; set; }

        public int? Take { get; set; }

        public IReflectorMetadata GetAccesor()
        {
            return __accesor;
        }

        public sealed class GetAllRequestAccesor : IReflectorMetadata
        {
            public const int Filter = 1;
            public const int Sort = 2;
            public const int Skip = 3;
            public const int Take = 4;
            private readonly int[] _props = new[]
            {
            Filter,Sort,Skip,Take
        };

            public int[] GetPropertyCodes() => _props;

            public string GetPropertyName(int propertyCode)
            {
                return propertyCode switch
                {
                    Filter => "Filter",
                    Sort => "Sort",
                    Skip => "Skip",
                    Take => "Take",

                    _ => throw new InvalidOperationException("property code not supported"),
                };
            }

            public int GetPropertyCode(string propertyName)
            {
                return propertyName switch
                {
                    "Filter" => Filter,
                    "Sort" => Sort,
                    "Skip" => Skip,
                    "Take" => Take,

                    _ => -1,
                };
            }

            public Type GetPropertyType(int propertyCode)
            {
                return propertyCode switch
                {
                    Filter => typeof(string),
                    Sort => typeof(string),
                    Skip => typeof(int?),
                    Take => typeof(int?),

                    _ => throw new InvalidOperationException("property code not supported"),
                };
            }

            public object GetValue(object target, int propertyCode)
            {
                GetAllEntitiesRequest obj = (GetAllEntitiesRequest)target;
                return propertyCode switch
                {
                    Filter => obj.Filter,
                    Sort => obj.Sort,
                    Skip => obj.Skip,
                    Take => obj.Take,

                    _ => throw new InvalidOperationException("property code not supported"),
                };
            }

            public void SetValue(object target, int propertyCode, object value)
            {
                GetAllEntitiesRequest obj = (GetAllEntitiesRequest)target;
                switch (propertyCode)
                {
                    case Filter: obj.Filter = (string)value; break;
                    case Sort: obj.Sort = (string)value; break;
                    case Skip: obj.Skip = (int?)value; break;
                    case Take: obj.Take = (int?)value; break;

                    default: throw new InvalidOperationException("property code not supported");
                }
            }

        }
    }
}
