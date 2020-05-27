#nullable enable

using Cybtans.Proto.AST;

namespace Cybtans.Proto.Options
{
    public abstract class SecurityOptions : ProtobufOption
    {
        public SecurityOptions(OptionsType type) : base(type)
        {

        }

        [Field("policy")]
        public string? Policy { get; set; }

        [Field("roles")]
        public string? Roles { get; set; }

        [Field("authorized")]
        public bool Authorized { get; set; }

        [Field("anonymous")]
        public bool AllowAnonymous { get; set; }

        public bool RequiredAuthorization => Authorized || Roles != null || Policy != null;
    }
}
