#nullable enable

using CybtansSdk.Proto.AST;

namespace CybtansSdk.Proto.Options
{
    public class FileOptions : ProtobufOption
    {
        public FileOptions() : base(OptionsType.File) { }

        [Field("namespace")]
        public string? Namespace { get; set; }
        
    }
}
