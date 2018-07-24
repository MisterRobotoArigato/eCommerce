using System.Collections.Generic;

namespace MisterRobotoArigato.Models
{
    /// <summary>
    /// A basket is connected to a user by their email
    /// A basket can have multiple BasketItems
    /// </summary>
    public class Basket
    {
        public int ID { get; set; }
        public string CustomerEmail { get; set; } //this will be tied to user email
        public List<BasketItem> BasketItems { get; set; }

        public Basket()
        {
            BasketItems = new List<BasketItem>();
        }
    }
}