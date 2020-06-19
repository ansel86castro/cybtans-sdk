#nullable enable

using Cybtans.Proto.AST;
using System;
using System.IO;

namespace Cybtans.Proto.Generators.CSharp
{
    public abstract class FileGenerator
    {
        protected TypeGeneratorOption _option;
        protected ProtoFile _proto;

        protected FileGenerator(ProtoFile proto, TypeGeneratorOption option)
        {
            _option = option;
            _proto = proto;
        }

        public abstract void GenerateCode();

        public CsFileWriter CreateWriter(string ns)
        {
            return new CsFileWriter(ns, _option.OutputDirectory ?? Environment.CurrentDirectory);
        }

        protected void CreateFile(string filename, CodeWriter code)
        {
            File.WriteAllText(filename, code.ToString());
        }

    }

   

    
}
