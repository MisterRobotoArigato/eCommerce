using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MisterRobotoArigato.Data
{
    public class RobotoDbContext : DbContext
    {
        public RobotoDbContext(DbContextOptions<RobotoDbContext> options) : base (options)
        {

        }

    }
}
