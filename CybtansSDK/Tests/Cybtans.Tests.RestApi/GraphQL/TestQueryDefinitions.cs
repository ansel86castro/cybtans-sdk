using GraphQL.Types;
using System;

namespace Cybtans.Tests.GraphQL
{
    partial class TestQueryDefinitions
    {
        public TestQueryDefinitions()
        {
            AddTestDefinitions();
        }
    }

    public class TestQueryDefinitionsSchema: Schema
    {
        public TestQueryDefinitionsSchema(IServiceProvider provider) : base(provider)
        {
            Query = new TestQueryDefinitions();
        }
    }
}
