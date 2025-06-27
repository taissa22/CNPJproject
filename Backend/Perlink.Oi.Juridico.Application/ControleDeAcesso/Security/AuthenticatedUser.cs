using Microsoft.AspNetCore.Http;
using Shared.Domain.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Perlink.Oi.Juridico.Application.Security {
    public class AuthenticatedUser : IAuthenticatedUser {
        private readonly IHttpContextAccessor _accessor;

        public AuthenticatedUser(IHttpContextAccessor accessor) {
            _accessor = accessor;
        }

#if DEBUG
        public string Login => _accessor.HttpContext.User.Identity.Name ?? "PERLINK";
#else
        public string Login => _accessor.HttpContext.User.Identity.Name;
#endif

        public string Name => GetClaimsIdentity().FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier)?.Value;

        public IEnumerable<Claim> GetClaimsIdentity() {
            return _accessor.HttpContext.User.Claims;
        }
    }
}