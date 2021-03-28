#nullable enable

using Cybtans.Proto.AST;

namespace Cybtans.Proto.Options
{
    public class FileOptions : ProtobufOption
    {
        string? _namespace;

        public FileOptions() : base(OptionsType.File) { }

        [Field("namespace")]
        public string? Namespace { get => _namespace; set => _namespace = value; }


        [Field("csharp_namespace")]
        public string? CSharpNamespace { get => _namespace; set => _namespace = value; }

    }
}
