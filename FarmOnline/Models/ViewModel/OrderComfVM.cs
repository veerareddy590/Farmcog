namespace FarmOnline.Models.ViewModel
{
    public class OrderComfVM
    {
        public OrderHeader OrderHeader { get; set; }
        public IEnumerable<OrderDetail> OrderDetails { get; set; }
    }
}
