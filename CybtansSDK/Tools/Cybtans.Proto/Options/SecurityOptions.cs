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

        public class Wrapper : ProtobufOption
        {
            private SecurityOptions _options;

            public Wrapper(SecurityOptions options) : base(options.Type)
            {
                _options = options;
            }

            [Field("policy")]
            public string? Policy { get => _options.Policy; set => _options.Policy = value; }

            [Field("roles")]
            public string? Roles { get => _options.Roles; set => _options.Roles = value; }

            [Field("authorized")]
            public bool Authorized { get => _options.Authorized; set => _options.Authorized = value; }

            [Field("anonymous")]
            public bool AllowAnonymous { get => _options.AllowAnonymous; set => _options.AllowAnonymous = value; }
        }
    }
}
