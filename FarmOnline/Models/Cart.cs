using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FarmOnline.Models
{
    public class Cart
    {

        public Cart() {
            count = 1;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Product")]
        [ValidateNever]
        
        public int ProductId { get; set; }

        [ForeignKey("User")]
        [ValidateNever]
        public string CustomerId { get; set; }

        [Required]
        [Range(1,100,ErrorMessage = "Please enter a value between 1 and 100")]
        public int count { get; set; }

        [ValidateNever]
        public Product Product { get; set; }

        [ValidateNever]
        public ApplicationUser User { get; set; }
    }
}
