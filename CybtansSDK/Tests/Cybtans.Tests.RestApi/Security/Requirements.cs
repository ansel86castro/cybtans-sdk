using Microsoft.AspNetCore.Authorization;

namespace Cybtans.Tests.RestApi.Security
{
    public class ClientPolicyRequirement:IAuthorizationRequirement
    {
        public override string ToString()
        {
            return "Invalid client_id";
        }
    }

    public class ClientCreatorRequiriment : IAuthorizationRequirement
    {
        public override string ToString()
        {
            return "Invalid creator id";
        }
    }
}
