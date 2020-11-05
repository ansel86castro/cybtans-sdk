using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Cybtans.Services.BaseServices
{
   
    public class CurrentUserProvider : ICurrentUserProvider
    {
        IHttpContextAccessor _accesor;
        public CurrentUserProvider(IHttpContextAccessor httpContext)
        {
            _accesor = httpContext;
        }

        public ClaimsPrincipal User => _accesor.HttpContext.User;

        public string UserId => _accesor.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        public string UserName => _accesor.HttpContext.User?.FindFirstValue(ClaimTypes.Name);
    }
}
