using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FarmOnline.Models.ViewModel
{
    public class ProductVM
    {
        [ValidateNever]
        public ProductFarmer ProductFarmer { get; set; }
        public Product Product { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> CategoryList { get; set; }
    }
}
