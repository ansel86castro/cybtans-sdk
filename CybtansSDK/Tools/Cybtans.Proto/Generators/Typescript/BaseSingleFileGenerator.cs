using Cybtans.Proto.AST;
using System;
using System.Collections.Generic;
using System.IO;

namespace Cybtans.Proto.Generators.Typescript
{
    public abstract class BaseSingleFileGenerator
    {
        ProtoFile _proto;
        TsOutputOption _option;
        readonly Dictionary<string, string> _blocks = new Dictionary<string, string>();

        public BaseSingleFileGenerator(ProtoFile proto, TsOutputOption option)
        {
            _proto = proto;
            _option = option;
        }

        public ProtoFile Proto => _proto;

        public TsOutputOption Options => _option;       

        public void GenerateCode()
        {
            Directory.CreateDirectory(_option.OutputPath);

            var writer = CreateWriter();
            OnGenerationBegin(writer);

            foreach (var item in _proto.ImportedFiles)
            {
                GenerateCode(item);
            }

            GenerateCode(_proto);

            OnGenerationEnd(writer);

            writer.Writer.Append(string.Join("\r\n", _blocks.Values));

            writer.Save(_option.Filename);
        }

        public virtual void OnGenerationBegin(TsFileWriter writer) { }

        public virtual void OnGenerationEnd(TsFileWriter writer) { }

        public TsFileWriter CreateWriter()
        {
            return new TsFileWriter(_option.OutputPath ?? Environment.CurrentDirectory);
        }

        protected abstract void GenerateCode(ProtoFile proto);

        protected void AddBlock(string name, string value)
        {
            _blocks[name] = value;
        }
    }
}
