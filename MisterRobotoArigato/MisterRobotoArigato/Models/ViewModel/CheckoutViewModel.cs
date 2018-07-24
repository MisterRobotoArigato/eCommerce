namespace MisterRobotoArigato.Models.ViewModel
{
    /// <summary>
    /// Makes it easier to display an object to a view for the checkout
    /// </summary>
    public class CheckoutViewModel
    {
        public Basket Basket { get; set; }
        public decimal DiscountPercent { get; set; }
        public string DiscountName { get; set; }
        public Address Address { get; set; }
        public string Shipping { get; set; }
        public int TotalItemQty { get; set; }
        public decimal Subtotal { get; set; }
        public decimal DiscountAmt { get; set; }
        public decimal Total { get; set; }

        public CheckoutViewModel()
        {
            Address = new Address();
        }
    }
}