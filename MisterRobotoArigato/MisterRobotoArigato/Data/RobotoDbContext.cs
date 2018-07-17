using Microsoft.EntityFrameworkCore;
using MisterRobotoArigato.Models;
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

        public DbSet<Product> Products { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<BasketDetail> BasketDetails { get; set; }
    }
}
