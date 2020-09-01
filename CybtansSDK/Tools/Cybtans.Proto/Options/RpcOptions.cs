#nullable enable

using Cybtans.Proto.AST;

namespace Cybtans.Proto.Options
{
    public class RpcOptions : SecurityOptions
    {
        public RpcOptions() : base(OptionsType.Rpc)
        {

        }

        [Field("template")]
        public string? Template { get; set; }
        
        [Field("method")]
        public string? Method { get; set; }        

        [Field("file")]
        public StreamOptions? StreamOptions { get; set; }

    }

    public class StreamOptions: ProtobufOption
    {
        public StreamOptions() : base(OptionsType.Rpc)
        {
        }

        [Field("contentType")]
        public string? ContentType { get; set; }

        [Field("name")]
        public string? Name { get; set; }
    }
}
