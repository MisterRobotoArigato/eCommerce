using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MisterRobotoArigato.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MisterRobotoArigato.Models
{
    public class StartupDbInitializer
    {
        private static readonly List<IdentityRole> Roles = new List<IdentityRole>()
        {
            new IdentityRole{Name = ApplicationRoles.Admin,
                NormalizedName = ApplicationRoles.Admin.ToUpper(),
                ConcurrencyStamp = Guid.NewGuid().ToString()},

            new IdentityRole{Name = ApplicationRoles.Member,
                NormalizedName = ApplicationRoles.Member.ToUpper(),
                ConcurrencyStamp = Guid.NewGuid().ToString()}
        };

        public static void SeedData(IServiceProvider serviceProvider, UserManager<ApplicationUser> userManager)
        {
            using (var dbContext =
                new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                dbContext.Database.EnsureCreated();
                AddRoles(dbContext);
            }
        }

        private static void AddRoles(ApplicationDbContext dbContext)
        {
            if (dbContext.Roles.Any()) return;
            foreach (var role in Roles)
            {
                dbContext.Roles.Add(role);
                dbContext.SaveChanges();
            }
        }
    }
}
