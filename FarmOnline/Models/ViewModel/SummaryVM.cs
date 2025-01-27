namespace FarmOnline.Models.ViewModel
{
    public class SummaryVM
    {
        public Address Address { get; set; }
        public IEnumerable<Cart> CartItems { get; set; }
        public double TotalAmount { get; set; }
    }
}
