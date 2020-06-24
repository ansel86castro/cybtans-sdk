using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace Cybtans.AspNetCore
{
    public class SwachBuckleOperationFilters : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var pathParameters = operation.Parameters.Where(x => x.In == ParameterLocation.Path).ToDictionary(x => x.Name);

            foreach (var item in (from p in operation.Parameters
                                  where p.In != ParameterLocation.Path && pathParameters.ContainsKey(p.Name.ToLowerInvariant())
                                  select p).ToList())
            {
                operation.Parameters.Remove(item);
            }

            if (operation.RequestBody?.Content?.Count > 0)
            {
                var schemaRef = operation.RequestBody.Content.FirstOrDefault().Value.Schema;
                if (schemaRef.Reference?.Id != null)
                {
                    var schema = context.SchemaRepository.Schemas[schemaRef.Reference.Id];
                    foreach (var item in schema.Properties.Where(x => pathParameters.ContainsKey(x.Key)).Select(x => x.Key).ToList())
                    {
                        schema.Properties.Remove(item);
                    }
                }
            }
        }
    }
}
