namespace MisterRobotoArigato.Models
{
    public class Checkout
    {
        public string CCNumber { get; set; }
        private Address Address { get; set; }
        public decimal Amount { get; set; }
    }
}