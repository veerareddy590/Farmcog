using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FarmOnline.Models
{
   
    public class User
    {

        [Key]
        public string UserId {  get; set; }

        [Required(ErrorMessage = "Name Must Be Filled")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email Must Be Filled")]
        [RegularExpression(@"^[a-zA-Z0-9+_.-]+@[a-zA-Z0-9.-]+$",ErrorMessage = "Email is Invalid")]
        public string Email { get; set; }

        [RegularExpression(@"[789]{1}[0-9]{9}",ErrorMessage = "Invalid Mobile Number")]
        [MaxLength(10)]
        
        public string Mobile { get; set; }

        [Required(ErrorMessage = "Passwords must be filled")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Select Farmer or User")]
        public string Roles { get; set; }

        [ValidateNever]
        public ICollection<ProductFarmer> ProductFarmers { get; set; }
       
        [ValidateNever]
        public ICollection<Cart> Carts { get; set; }

        [ValidateNever]
        public ICollection<Address> Addresses {  get; set; }    

        [ValidateNever]
        public ICollection<OrderHeader> OrderHeaders { get; set; }


    }
}
