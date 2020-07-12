﻿#nullable enable

using System.IO;

namespace Cybtans.Proto.Generators.CSharp
{
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
            SaveTo(_outputDirectory, name);            
        }

        public void SaveTo(string outputDirectory, string name)
        {
            File.WriteAllText(Path.Combine(outputDirectory, $"{name}.cs"), _fileWriter.ToString());
        }
    }

   

    
}
