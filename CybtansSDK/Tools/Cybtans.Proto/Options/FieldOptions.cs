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

        [Field("description")]
        public string? Description { get; set; }

        [Field("ts")]
        public TypecriptOptions Typecript { get; set; } = new TypecriptOptions();
    }

    public class TypecriptOptions : ProtobufOption
    {
        public TypecriptOptions() : base(OptionsType.Field){ }


        [Field("partial")]
        public bool Partial { get; set; }
    }
}
