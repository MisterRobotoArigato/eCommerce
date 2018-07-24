namespace MisterRobotoArigato.Models
{
    /// <summary>
    /// an Order can have many OrderItems
    /// </summary>
    public class OrderItem
    {
        public int ID { get; set; }
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public string UserID { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public string ImgUrl { get; set; }
        public decimal UnitPrice { get; set; }
    }
}