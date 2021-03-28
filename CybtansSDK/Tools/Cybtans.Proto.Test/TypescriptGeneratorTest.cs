using Cybtans.Proto.Generators.Typescript;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Cybtans.Proto.Test
{
    public class TypescriptGeneratorTest
    {
        [Theory]
        [InlineData("Protos/Service1.proto", "Typecript/Service1")]
        [InlineData("Protos/Catalog.proto", "Typecript/Catalog")]
        [InlineData("Protos/Customers.proto", "Typecript/Customer")]
        [InlineData("Protos/Compatibility.proto", "Typecript/Compatibility")]
        public void GenerateCode(string filename, string output)
        {
            var fileResolverFactory = new SearchPathFileResolverFactory(new string[] { "Proto" });

            Proto3Generator generator = new Proto3Generator(fileResolverFactory);
            var (ast, scope) = generator.LoadFromFile(filename);
            Assert.NotNull(ast);

            TypescriptGenerator tsGenerator = new TypescriptGenerator(new TypescriptOptions
            {
                ModelOptions = new TsOutputOption
                {
                    OutputPath = output,
                },
                ClientOptions = new TsOutputOption
                {
                    OutputPath = output
                }
            });

            tsGenerator.GenerateCode(ast);
        }

        [Theory]
        [InlineData("Protos/Service1.proto", "Angular/Service1")]
        [InlineData("Protos/Catalog.proto", "Angular/Catalog")]
        [InlineData("Protos/Customers.proto", "Angular/Customer")]
        [InlineData("Protos/Compatibility.proto", "Angular/Compatibility")]
        public void GenerateAngularCode(string filename, string output)
        {
            var fileResolverFactory = new SearchPathFileResolverFactory(new string[] { "Proto" });

            Proto3Generator generator = new Proto3Generator(fileResolverFactory);
            var (ast, scope) = generator.LoadFromFile(filename);
            Assert.NotNull(ast);

            TypescriptGenerator tsGenerator = new TypescriptGenerator(new TypescriptOptions
            {
                ModelOptions = new TsOutputOption
                {
                    OutputPath = output,
                },
                ClientOptions = new TsOutputOption
                {
                    OutputPath = output,
                    Framework = TsOutputOption.FRAMEWORK_ANGULAR
                }
            });

            tsGenerator.GenerateCode(ast);
        }


    }
}
