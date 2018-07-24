using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MisterRobotoArigato.Models.Handlers
{
    /// <summary>
    /// This is to enforce the "IsDoge" policy cause only doges get special privileges
    /// </summary>
    public class IsDogeHandler : AuthorizationHandler<IsDogeRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsDogeRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == ClaimTypes.Email))
            {
                return Task.CompletedTask;
            }

            string userEmail = context.User.FindFirst(e => e.Type == ClaimTypes.Email).Value;

            if (userEmail.Contains(requirement.Email))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
