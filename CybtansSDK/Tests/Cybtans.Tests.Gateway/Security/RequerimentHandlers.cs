using Cybtans.Tests.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration.UserSecrets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cybtans.Tests.Gateway.Security
{
    public class ClientPolicyHandlers : AuthorizationHandler<ClientPolicyRequirement, ClientRequest>
    {

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ClientPolicyRequirement requirement, ClientRequest resource)
        {
            if (context.User.HasClaim(x => x.Type == "client_id"))
            {
                var userId = Guid.Parse(context.User.Claims.First(x => x.Type == "client_id").Value);
                if (resource.Id == userId)
                {
                    context.Succeed(requirement);
                }
            }

            if (resource.Id == Guid.Empty)
            {
                resource.Id = Guid.Parse("D6E29710-B68F-4D2D-9471-273DECF9C4B7");
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }

    public class ClientCreatorPolicyHandler : AuthorizationHandler<ClientCreatorRequiriment, ClientDto>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ClientCreatorRequiriment requirement, ClientDto resource)
        {
            if (context.User.HasClaim(x => x.Type == "creator_id"))
            {
                var creatorId = int.Parse(context.User.Claims.First(x => x.Type == "creator_id").Value);
                if (creatorId == resource.CreatorId)
                {
                    context.Succeed(requirement);
                }
            }

            return Task.CompletedTask;
        }
    }
}
