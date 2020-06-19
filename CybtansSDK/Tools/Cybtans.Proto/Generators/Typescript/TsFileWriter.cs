using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Cybtans.Proto.Generators.Typescript
{
    public class TsFileWriter
    {
        private readonly string _outputDirectory;
        private readonly CodeWriter _fileWriter;

        public TsFileWriter(string outputDirectory)
        {
            _fileWriter = new CodeWriter();
            this._outputDirectory = outputDirectory;

        }

        public CodeWriter Writer => _fileWriter;

        public void Save(string name)
        {
            File.WriteAllText(Path.Combine(_outputDirectory, $"{name}.ts"), _fileWriter.ToString());
        }
    }
}
