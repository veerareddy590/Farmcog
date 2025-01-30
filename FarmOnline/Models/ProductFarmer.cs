using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FarmOnline.Models
{
    public class ProductFarmer
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }


        [ForeignKey("Product")]
        public int ProductId { get; set; }

        [ForeignKey("User")]
        public string FarmerId { get; set; }

        [ValidateNever]
        public Product Product { get; set; }

        [ValidateNever]
        public ApplicationUser User { get; set; }


    }
}
