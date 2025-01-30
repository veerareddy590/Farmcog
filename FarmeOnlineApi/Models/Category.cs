using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FarmOnlineApi.Models
{
    public class Category
    {
        [Key]
        public int CategoryId {  get; set; }

        [Required(ErrorMessage = "Category Name must be required")]
        public string CategoryName { get; set; }

    }
}
