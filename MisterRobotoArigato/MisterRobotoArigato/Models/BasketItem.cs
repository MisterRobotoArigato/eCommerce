namespace MisterRobotoArigato.Models
{
    /// <summary>
    /// every Basket object can have multiple BasketItems
    /// this will be linked to the Basket object
    /// </summary>
    public class BasketItem
    {
        public int ID { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string CustomerEmail { get; set; } //this will be tied to user email
        public int Quantity { get; set; }
        public string ImgUrl { get; set; }
        public decimal UnitPrice { get; set; }
    }
}