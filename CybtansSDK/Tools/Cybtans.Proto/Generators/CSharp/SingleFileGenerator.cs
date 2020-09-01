#nullable enable

using Cybtans.Proto.AST;
using System.Collections.Generic;
using System.IO;

namespace Cybtans.Proto.Generators.CSharp
{
    public abstract class SingleFileGenerator<T>:FileGenerator<T>
        where T: CodeGenerationOption
    {       
        protected readonly Dictionary<string, string> _blocks = new Dictionary<string, string>();
        private string _namespace;

        public SingleFileGenerator(ProtoFile proto, T option, string ns):base(proto, option)
        {
            _namespace = ns;
        }
     
        public override void GenerateCode()
        {
            Directory.CreateDirectory(_option.OutputPath);

            var writer = CreateWriter(_namespace);
            OnGenerationBegin(writer);

            foreach (var item in Proto.ImportedFiles)
            {
                GenerateCode(item);
            }

            GenerateCode(Proto);

            OnGenerationEnd(writer);

            writer.Class.Append(string.Join("\r\n", _blocks.Values));

            SaveFile(writer);
        }

        protected abstract void SaveFile(CsFileWriter writer);

        public virtual void OnGenerationBegin(CsFileWriter writer) { }

        public virtual void OnGenerationEnd(CsFileWriter writer) { }
     
        protected void AddBlock(string name, string value)
        {
            _blocks[name] = value;
        }
    }


}
