using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Cybtans.AspNetCore
{
    public class SwachBuckleOperationFilters : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var pathParameters = operation
                .Parameters
                .Where(x => x.In == ParameterLocation.Path)
                .ToDictionary(x => x.Name.ToLowerInvariant());

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
                    foreach (var item in schema.Properties
                        .Where(x => pathParameters.ContainsKey(x.Key.ToLowerInvariant()))
                        .Select(x => x.Key).ToList())
                    {
                        schema.Properties.Remove(item);
                    }
                    
                }
            }            
        }
    }

    public class SwachBuckleSchemaFilters : ISchemaFilter
    {           

        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema.Properties.Count == 0 || context.MemberInfo != null)
            {               
                return;
            }

            var props = context.Type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var p in props.Where(x => x.PropertyType == typeof(Stream)))
            {
                var prop = schema.Properties.FirstOrDefault(x => x.Key.ToLowerInvariant() == p.Name.ToLowerInvariant()).Key;
                if (prop != null)
                    schema.Properties.Remove(prop);
            }                       
        }        
    }

   
}
