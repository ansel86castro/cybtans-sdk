using Cybtans.Services;
using FluentValidation.Results;
using Grpc.Core;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

#nullable enable

namespace Cybtans.Grpc
{
    public static class UtilityExtensions
    {
        private const string ValidationMetadataKey = "validation-errors-text";

        public static Metadata ToValidationMetadata(this ValidationException e)
        {
            var metadata = new Metadata();
            Services.ValidationResult? failures = e.ValidationResult;
            if (failures != null)
            {
                metadata.Add(new Metadata.Entry(ValidationMetadataKey, JsonSerializer.Serialize(failures)));
            }
            return metadata;
        }


        public static Metadata ToValidationMetadata(this IEnumerable<ValidationFailure> failures)
        {
            Services.ValidationResult err = new Services.ValidationResult("Request Not Valid");
            var metadata = new Metadata();

            if (failures?.Any() ?? false)
            {
                foreach (var item in failures.GroupBy(x=>x.PropertyName))
                {
                    err.AddError(item.Key, string.Join(",", item.Select(x => x.ErrorMessage)));
                }

                metadata.Add(new Metadata.Entry(ValidationMetadataKey, JsonSerializer.Serialize(err)));
            }

            return metadata;
        }

        public static IEnumerable<ValidationTrailers> ToValidationTrailers(this IEnumerable<ValidationFailure> failures)
        {
            return failures.Select(x => new ValidationTrailers
            {
                PropertyName = x.PropertyName,
                AttemptedValue = x.AttemptedValue?.ToString(),
                ErrorMessage = x.ErrorMessage
            }).ToList();
        }


        public static bool HasValidationFailures(this RpcException ex)
        {
            return ex.StatusCode == StatusCode.InvalidArgument && ex.Trailers.Any(x => x.Key == ValidationMetadataKey);
        }

        public static Services.ValidationResult? GetValidationResult(this RpcException ex)
        {
            if (ex.StatusCode != StatusCode.InvalidArgument)
                return null;

            var meta = ex.Trailers.FirstOrDefault(x => x.Key == ValidationMetadataKey);
            if (meta == null) return null;

            return JsonSerializer.Deserialize<Services.ValidationResult>(meta.Value);
        }
    }
}
