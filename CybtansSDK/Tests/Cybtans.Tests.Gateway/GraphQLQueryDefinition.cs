using GraphQL.Types;
using System;

namespace Cybtans.Tests.Gateway.GraphQL
{
    partial class GraphQLQueryDefinitions
    {
        public GraphQLQueryDefinitions()
        {
            AddTestDefinitions();

            AddGreetDefinitions();
        }
    }

    public class ApiGatewayDefinitionsSchema : Schema
    {
        public ApiGatewayDefinitionsSchema(IServiceProvider provider)
            : base(provider)
        {
            Query = new GraphQLQueryDefinitions();
        }
    }

}
