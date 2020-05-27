#nullable enable

using Cybtans.Proto.AST;

namespace Cybtans.Proto.Options
{

    public class ServiceOptions: SecurityOptions
    {
        public ServiceOptions() : base(OptionsType.Service) { }

        [Field("prefix")]
        public string? Prefix { get; set; }      

        [Field("context")]
        public string? ContextClass { get; set; }        
    }
}
