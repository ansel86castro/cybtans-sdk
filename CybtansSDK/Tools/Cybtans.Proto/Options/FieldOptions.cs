#nullable enable

using Cybtans.Proto.AST;

namespace Cybtans.Proto.Options
{
    public class FieldOptions: ProtobufOption
    {
        public FieldOptions() : base( OptionsType.Field) { }

        [Field("required")]
        public bool Required { get; set; }

        [Field("optional")]
        public bool Optional { get; set; }

        [Field("deprecated")]
        public bool Deprecated { get; set; }

        [Field("default")]
        public object? Default { get; set; }
    }
}
