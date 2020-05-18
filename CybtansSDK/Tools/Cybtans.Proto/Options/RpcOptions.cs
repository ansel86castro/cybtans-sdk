#nullable enable

using Cybtans.Proto.AST;

namespace Cybtans.Proto.Options
{
    public class RpcOptions : ProtobufOption
    {
        public RpcOptions() : base(OptionsType.Rpc)
        {

        }

        [Field("template")]
        public string? Template { get; set; }
        
        [Field("method")]
        public string? Method { get; set; }

        [Field("claims")]
        public string? Claims { get; set; }

    }
}
