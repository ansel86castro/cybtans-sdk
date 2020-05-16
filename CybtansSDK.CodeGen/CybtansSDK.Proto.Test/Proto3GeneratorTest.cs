using CybtansSdk.Proto.AST;
using CybtansSdk.Proto.Generators.CSharp;
using CybtansSdk.Proto.Options;
using System;
using System.Linq;
using Xunit;

namespace CybtansSdk.Proto.Test
{
    public class Proto3GeneratorTest
    {
        [Fact]
        public void LoadProtoFromFileNoImports()
        {
            var fileResolverFactory = new SearchPathFileResolverFactory(new string[] { "Proto" });

            Proto3Generator generator = new Proto3Generator(fileResolverFactory);
            var (ast, scope) = generator.LoadFromFile("Protos/Service1.proto");

            AssertAST(ast);
        }

        [Fact]
        public void GenerateCode()
        {
            var fileResolverFactory = new SearchPathFileResolverFactory(new string[] { "Proto" });

            Proto3Generator generator = new Proto3Generator(fileResolverFactory);
            var (ast, scope) = generator.LoadFromFile("Protos/Service1.proto");

            AssertAST(ast);

            MicroserviceGenerator microserviceGenerator = new MicroserviceGenerator(new GenerationOptions
            {
                ModelOptions = new TypeGeneratorOption
                {
                      OutputDirectory = "Generated"
                },
                ServiceOptions = new TypeGeneratorOption
                {
                     OutputDirectory = "Generated"
                },
                ControllerOptions = new TypeGeneratorOption
                {
                     OutputDirectory = "Generated"
                }
            });

            microserviceGenerator.GenerateCode(ast);
        }

        private static void AssertAST(ProtoFile ast)
        {
            Assert.NotNull(ast);
            Assert.NotNull(ast.Option.Namespace);
            Assert.NotNull(ast.Declarations);
            Assert.Equal(6, ast.Declarations.Count);
            Assert.NotNull(ast.Package);
            Assert.Equal("Service1", ast.Package.ToString());
            Assert.Equal(4, ast.Declarations.Where(x => x is MessageDeclaration).Count());
            Assert.Single(ast.Declarations.Where(x => x is ServiceDeclaration));

            foreach (var msg in ast.Declarations.Where(x => x is MessageDeclaration).Select(x => (MessageDeclaration)x))
            {
                Assert.NotNull(msg.Option);
                Assert.NotEmpty(msg.Fields);
                foreach (var field in msg.Fields)
                {
                    Assert.NotNull(field.Name);
                    Assert.NotNull(field.Option);
                    Assert.True(field.Number > 0);

                    Assert.NotNull(field.Type);
                    Assert.NotNull(field.Type.Name);
                    Assert.NotNull(field.Type.TypeDeclaration);

                    if (field.Name == "preferences")
                    {
                        Assert.True(field.Type.IsArray);
                    }
                }
            }

            foreach (var srv in ast.Declarations.Where(x => x is ServiceDeclaration).Select(x => (ServiceDeclaration)x))
            {
                Assert.NotNull(srv.Option);
                Assert.NotNull(srv);
                Assert.NotEmpty(srv.Rpcs);

                var serviceOptions = srv.Option as ServiceOptions;

                Assert.NotNull(serviceOptions);
                Assert.Equal("api/service1", serviceOptions.Prefix);

                foreach (var rpc in srv.Rpcs)
                {
                    Assert.NotNull(rpc.Request);
                    Assert.NotNull(rpc.Response);

                    Assert.NotNull(rpc.RequestType);
                    Assert.NotNull(rpc.ResponseType);

                    var rpcOptions = rpc.Option as RpcOptions;
                    Assert.NotNull(rpcOptions);
                    Assert.NotNull(rpcOptions.Method);
                }
            }
        }
    }
}
