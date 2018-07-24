using Microsoft.AspNetCore.Identity;

namespace MisterRobotoArigato.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    /// <summary>
    /// these roles are accessible w/o having to instantiate ApplicationUser
    /// these roles are used to enforce policies
    /// </summary>
    public static class ApplicationRoles
    {
        public const string Member = "Member";
        public const string Admin = "Admin";
        public const string CatLady = "CatLady";
    }
}