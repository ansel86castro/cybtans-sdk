#nullable enable

using CybtansSdk.Proto.AST;

namespace CybtansSdk.Proto.Options
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
