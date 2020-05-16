#nullable enable

using CybtansSdk.Proto.AST;

namespace CybtansSdk.Proto.Options
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
