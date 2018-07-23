using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MisterRobotoArigato.Models
{
    public class Order
    {
        public int ID { get; set; }
        public string UserID { get; set; }
        public string Shipping { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public int AddressID { get; set; }
        public Address Address { get; set; }
        public string DiscountName { get; set; }
        public decimal DiscountPercent { get; set; }
    }
}
