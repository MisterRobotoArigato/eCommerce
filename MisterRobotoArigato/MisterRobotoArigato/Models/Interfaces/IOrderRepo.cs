using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MisterRobotoArigato.Models
{
    public interface IOrderRepo
    {
        Task<List<Order>> GetRecentOrdersAsync();
    }
}
