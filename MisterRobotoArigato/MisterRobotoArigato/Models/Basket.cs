using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MisterRobotoArigato.Models
{
    public class Basket
    {
        public int ID { get; set; }
        public string CustomerEmail { get; set; } //this will be tied to user email
        public List<Product> Products { get; set; }
    }
}
