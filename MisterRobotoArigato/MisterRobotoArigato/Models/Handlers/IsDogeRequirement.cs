using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MisterRobotoArigato.Models.Handlers
{
    /// <summary>
    /// Associate a doge by their email
    /// </summary>
    public class IsDogeRequirement : IAuthorizationRequirement
    {
        public string Email { get; set; }

        public IsDogeRequirement(string email)
        {
            Email = email;
        }
    }
}
