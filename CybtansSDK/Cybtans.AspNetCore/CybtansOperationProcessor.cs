using NSwag;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;
using System.Linq;

namespace Cybtans.AspNetCore
{
    public class CybtansOperationProcessor : IOperationProcessor
    {
        public bool Process(OperationProcessorContext context)
        {
            var operation = context.OperationDescription.Operation;
            var pathParameters = operation.Parameters.Where(x => x.Kind == OpenApiParameterKind.Path).ToDictionary(x => x.Name);

            foreach (var item in (from p in operation.Parameters
                                  where p.Kind != OpenApiParameterKind.Path && pathParameters.ContainsKey(p.Name.ToLowerInvariant())
                                  select p).ToList())
            {
                operation.Parameters.Remove(item);
            }

            foreach (var p in operation.Parameters.Where(x => x.Kind == OpenApiParameterKind.Body && x.ActualSchema != null))
            {
                foreach (var item in (from kv in p.ActualSchema.Properties
                                      where pathParameters.ContainsKey(kv.Key)
                                      select kv.Key).ToList())
                {
                    p.ActualSchema.Properties.Remove(item);
                }
            }

            return true;
        }
    }
}
