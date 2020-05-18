#nullable enable

using Cybtans.Proto.AST;

namespace Cybtans.Proto.Options
{
    public class EnumOptions : ProtobufOption
    {
        public EnumOptions() : base(OptionsType.Enum)
        {
        }

        [Field("deprecated")]
        public bool Deprecated { get; set; }

        [Field("value")]
        public int Value { get; set; }
    }
}
