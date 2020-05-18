#nullable enable

using Cybtans.Proto.AST;

namespace Cybtans.Proto.Options
{
    public class FileOptions : ProtobufOption
    {
        public FileOptions() : base(OptionsType.File) { }

        [Field("namespace")]
        public string? Namespace { get; set; }
        
    }
}
