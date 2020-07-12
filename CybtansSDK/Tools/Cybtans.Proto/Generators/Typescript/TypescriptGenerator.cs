using Cybtans.Proto.AST;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cybtans.Proto.Generators.Typescript
{
    public class TypescriptOptions
    {
        public TsOutputOption ModelOptions { get; set; }
        public TsOutputOption ClientOptions { get; set; }
    }

    public class TypescriptGenerator :ICodeGenerator
    {
        TypescriptOptions _options;

        public TypescriptGenerator(TypescriptOptions options)
        {
            _options = options;
        }

        public void GenerateCode(ProtoFile proto, Scope? scope = null)
        {           
            new TypeGenerator(proto, _options.ModelOptions).GenerateCode();
            if (_options.ClientOptions.Framework == TsOutputOption.FRAMEWORK_ANGULAR)
            {
                new AngularClientGenerator(proto, _options.ClientOptions).GenerateCode();
            }
            else
            {
                new ClientGenerator(proto, _options.ClientOptions).GenerateCode();
            }
        }
    }
}
