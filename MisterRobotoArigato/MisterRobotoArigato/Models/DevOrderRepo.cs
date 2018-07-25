using Microsoft.EntityFrameworkCore;
using MisterRobotoArigato.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MisterRobotoArigato.Models
{
    public class DevOrderRepo : IOrderRepo
    {
        private RobotoDbContext _context;

        public DevOrderRepo(RobotoDbContext context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetRecentOrdersAsync()
        {
            List<Order> last20Orders = last20Orders = await _context.Orders.Skip(Math.Max(0, _context.Orders.Count() - 20)).ToListAsync();            
            return last20Orders;
        }
    }
}
