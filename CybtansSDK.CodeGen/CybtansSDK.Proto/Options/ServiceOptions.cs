#nullable enable

using CybtansSdk.Proto.AST;

namespace CybtansSdk.Proto.Options
{
    public class ServiceOptions: ProtobufOption
    {
        public ServiceOptions() : base(OptionsType.Service) { }

        [Field("prefix")]
        public string? Prefix { get; set; }

        [Field("claims")]
        public string? Claims { get; set; }

        [Field("context")]
        public string ContextClass { get; set; }
        
    }
}
