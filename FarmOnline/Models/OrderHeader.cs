using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FarmOnline.Models
{
    public class OrderHeader
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("User")]
        [ValidateNever]
        public string UserId { get; set; }

        public DateTime OrderDate { get; set; }

        public DateTime ShippingDate { get; set; }

        public double OrderTotal { get; set; }

        public string OrderStatus { get; set; } = "PlacedOrder"; 

        public int PaymentStatus { get; set; }

        public string? PaymentIntentId { get; set; }

        [ValidateNever]
        public ApplicationUser User { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
