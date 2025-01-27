
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FarmOnline.Models
{
    public class Product
    {

        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ProductId { get; set; }

        [Required(ErrorMessage ="Name must be filled ")]
        public string Name { get; set; }

        
        public string Description { get; set; }

        [Required(ErrorMessage ="quantity must be filled")]
        public double Quantity { get; set; }

        [Required(ErrorMessage = "Price must be filled")]
        public double Price { get; set; }

        [ForeignKey("Category")]
        [ValidateNever]
        public int CategoryId { get; set; }


        [ValidateNever]
        [Required(ErrorMessage = "Upload a image")]

        public string Image {  get; set; }

        [ValidateNever]
        public Category Category { get; set; }

        
        [ValidateNever]
        public ICollection<Cart> carts { get; set; }
        [ValidateNever]
        public ICollection<ProductFarmer> productFarmers { get; set; }
        
    }
}
