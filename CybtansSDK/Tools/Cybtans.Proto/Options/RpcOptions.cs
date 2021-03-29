#nullable enable

using Cybtans.Proto.AST;

namespace Cybtans.Proto.Options
{
    public class RpcOptions : SecurityOptions
    {
        public RpcOptions() : base(OptionsType.Rpc)
        {
            RpcSecurity = new Wrapper(this);
        }

        [Field("template")]
        public string? Template { get; set; }
        
        [Field("method")]
        public string? Method { get; set; }        

        [Field("file")]
        public StreamOptions? StreamOptions { get; set; }

        [Field("description")]
        public string? Description { get; set; }

        [Field("rpc_description")]
        public string? RpcDescription { get => Description; set => Description = value; }

        [Field("google")]
        public GoogleHttpOptions? Google { get; set; }

        [Field("rpc_security")]
        public SecurityOptions.Wrapper RpcSecurity { get; }

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

    public class GoogleHttpOptions: ProtobufOption
    {
        public GoogleHttpOptions() : base(OptionsType.Rpc) { }

        [Field("api")]
        public GoogleApi? Api { get; set; }
    }

    public class GoogleApi : ProtobufOption
    {
        public GoogleApi() : base(OptionsType.Rpc) { }

        [Field("http")]
        public GoogleApi? Http { get; set; }
    }
}
