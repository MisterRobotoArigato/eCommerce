using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MisterRobotoArigato.Models
{
    /// <summary>
    /// Every order is connected to a user and a user can have multiple orders
    /// An Order will have a list of OrderItems, which is comprised of Products
    /// </summary>
    public class Order
    {
        [Display(Name="Order ID")]
        public int ID { get; set; }
        public List<OrderItem> OrderItems { get; set; }

        [Display(Name = "Customer ID")]
        public string UserID { get; set; }

        [Display(Name = "Shipping Method")]
        public string Shipping { get; set; }

        [Display(Name = "Address ID")]
        public int AddressID { get; set; }

        public Address Address { get; set; }

        [Display(Name = "Order Date")]
        public string OrderDate { get; set; }

        [Display(Name = "Total Item Quantity")]
        public int TotalItemQty { get; set; }

        [Display(Name = "Discount Name")]
        public string DiscountName { get; set; }

        [Display(Name = "Discount Percent")]
        public decimal DiscountPercent { get; set; }

        [Display(Name = "Discount Amount")]
        public decimal DiscountAmt { get; set; }

        public decimal Subtotal { get; set; }

        public decimal Total { get; set; }
    }
}
