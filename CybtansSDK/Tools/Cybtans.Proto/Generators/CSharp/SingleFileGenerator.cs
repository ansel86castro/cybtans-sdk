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
        private IEnumerable<ProtoFile> _protos;

        public SingleFileGenerator(ProtoFile entryPoint, IEnumerable<ProtoFile> protos, T option, string ns):base(entryPoint, option)
        {
            _namespace = ns;
            _protos = protos;
        }
     
        public override void GenerateCode()
        {
            Directory.CreateDirectory(_option.OutputPath);

            var writer = CreateWriter(_namespace);
            OnGenerationBegin(writer);

            foreach (var item in _protos)
            {
                GenerateCode(item);
            }          

            OnGenerationEnd(writer);

            writer.Class.Append(string.Join("\r\n", _blocks.Values));

            SaveFile(writer);
        }

        public IEnumerable<ProtoFile> Protos => _protos;

        protected abstract void SaveFile(CsFileWriter writer);

        public virtual void OnGenerationBegin(CsFileWriter writer) { }

        public virtual void OnGenerationEnd(CsFileWriter writer) { }
     
        protected void AddBlock(string name, string value)
        {
            _blocks[name] = value;
        }
    }


}
