#nullable enable

using CybtansSdk.Proto.AST;
using System;
using System.IO;

namespace CybtansSdk.Proto.Generators.CSharp
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


    public class CsFileWriter
    {
        private readonly CodeWriter _fileWriter;
        private readonly CodeWriter _usingWriter;
        private readonly CodeWriter _classWriter;
        private readonly string _outputDirectory;

        public CsFileWriter(string ns, string outputDirectory)
        {
            _fileWriter = new CodeWriter();
            this._outputDirectory = outputDirectory;

            _fileWriter.Append("using System;").AppendLine();
            _usingWriter = _fileWriter.Block("Using");
            _fileWriter.AppendLine();

            _fileWriter
                .Append($"namespace {ns}").AppendLine()
                .Append("{")
                .AppendLine()
                .Append('\t', 1);

           _classWriter = _fileWriter.Block("CLASS");

            _fileWriter.AppendLine()              
                .Append("}")
                .AppendLine();               
        }

        public CodeWriter Block(string name) => _fileWriter.Block(name);

        public CodeWriter Class => _classWriter;

        public CodeWriter Usings => _usingWriter;

        public void Save(string name)
        {
            File.WriteAllText(Path.Combine(_outputDirectory, $"{name}.cs"), _fileWriter.ToString());
        }
    }

   

    
}
