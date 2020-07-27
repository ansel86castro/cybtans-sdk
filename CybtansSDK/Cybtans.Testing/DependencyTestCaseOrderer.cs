using System.Collections.Generic;
using System.Linq;
using Cybtans.Tests.Core.Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Cybtans.Testing
{
    public class DependencyTestCaseOrderer : ITestCaseOrderer
    {
        class TestItem<TTestCase> where TTestCase : ITestCase
        {
            public TTestCase Test;

            public List<TTestCase> Dependencies = new List<TTestCase>();

            public int Order { get; set; }

            public string Name => Test.TestMethod.Method.Name;

            public override string ToString()
            {
                return Test.TestMethod.Method.Name;
            }
        }

        public IEnumerable<TTestCase> OrderTestCases<TTestCase>(IEnumerable<TTestCase> testCases)
            where TTestCase : ITestCase
        {
            var testLookup = testCases.GroupBy(x => x.TestMethod.Method.Name).ToDictionary(x => x.Key);
            List<TestItem<TTestCase>> testItems = new List<TestItem<TTestCase>>(testCases.Count());

            foreach (var t in testCases)
            {
                TestItem<TTestCase> node = new TestItem<TTestCase> { Test = t };
                var attr = t.TestMethod.Method.GetCustomAttributes(typeof(DependsOnAttribute).AssemblyQualifiedName).SingleOrDefault();
                if (attr != null)
                {
                    var dependencies = (string[])attr.GetConstructorArguments().FirstOrDefault();
                    foreach (var dep in dependencies)
                    {
                        if (testLookup.ContainsKey(dep))
                        {
                            node.Dependencies.AddRange(testLookup[dep]);
                        }
                    }
                }

                attr = t.TestMethod.Method.GetCustomAttributes(typeof(TestOrderAttribute).AssemblyQualifiedName).SingleOrDefault();
                if (attr != null)
                {
                    node.Order = (int)attr.GetConstructorArguments().FirstOrDefault();
                }

                testItems.Add(node);
            }

            testItems.Sort((x, y) => x.Order.CompareTo(y.Order));

            if (testItems.Any(x => x.Dependencies.Any()))
            {
                Dictionary<string, List<TTestCase>> graph = testItems.GroupBy(x => x.Name).ToDictionary(x => x.Key, x => x.First().Dependencies);
                var sort = TopologicalSort.Sort(testItems.Select(x => x.Test), t => graph[t.TestMethod.Method.Name]);
                return sort;
            }

            return testItems.Select(x => x.Test);
        }
    }
}
