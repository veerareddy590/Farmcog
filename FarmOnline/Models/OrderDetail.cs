using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FarmOnline.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }
        [Required]
        [ForeignKey("OrderHeader")]
        [ValidateNever]
        public int OrderId { get; set; }
        [Required]
        [ForeignKey("Product")]
        [ValidateNever]
        public int ProductId { get; set; }

        public int Count { get; set; }

        public double Price { get; set; }

        [ValidateNever]
        public OrderHeader OrderHeader { get; set; }

        [ValidateNever]
        public Product Product { get; set; }
    }
}
