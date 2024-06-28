using ClassMate.Domain.Abstractions.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ClassMate.Services
{
    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _contextAccesor;

        public UserContextService(IHttpContextAccessor contextAccesor)
        {
            _contextAccesor = contextAccesor;
        }

        public Guid GetCurrentUserID()
        {
            var identity = _contextAccesor.HttpContext.User.Identity as ClaimsIdentity;
            return Guid.Parse(identity!.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        }
    }
}
