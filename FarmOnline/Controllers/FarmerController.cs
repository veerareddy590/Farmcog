
using FarmOnline.Models;
using FarmOnline.Models.ViewModel;
using FarmOnline.Repositories;
using FarmOnline.Repositories.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FarmOnline.Controllers
{
    public class FarmerController : Controller
    {
        public IUnitofWork unitofWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public FarmerController(IUnitofWork unitofWork, IWebHostEnvironment _webHostEnvironment)
        {
            this.unitofWork = unitofWork;
            this._webHostEnvironment = _webHostEnvironment;
        }
        public IActionResult Index()
        {
            ViewBag.farmer = HttpContext.Session.GetString("UserId");

            return View();
        }

        [HttpGet]
        public IActionResult Product()
        {
            var farmerId = HttpContext.Session.GetString("UserId");
            var allproducts = unitofWork.product.GetAll();
            var prodcutid = unitofWork.productFarmer.GetAll().Where(x => x.FarmerId == farmerId);
            var products = (from product in allproducts
                            join productids in prodcutid on product.ProductId equals productids.ProductId
                            select product).ToList();

            return View(products);
        }

        [HttpGet]
        public IActionResult UploadProduct()
        {
            IEnumerable<Category> categories = (IEnumerable<Category>)unitofWork.category.Getall();
            if (categories == null)
            {

                return View("Error");
            }

            ProductVM productVM = new()
            {
                CategoryList = categories.Select(u => new SelectListItem
                {
                    Text = (string)u.CategoryName,
                    Value = u.CategoryId.ToString()
                }),
                Product = new Product()
            };

            return View(productVM);
        }

        [HttpPost]
        public IActionResult UploadProduct(ProductVM obj, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwroothpath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productpath = Path.Combine(wwwroothpath, @"images\product");
                    using (var fileStream = new FileStream(Path.Combine(productpath, filename), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    obj.Product.Image = @"\images\product\" + filename;
                }
                if (obj.ProductFarmer == null)
                {
                    obj.ProductFarmer = new ProductFarmer();
                }
                unitofWork.product.Add(obj.Product);
                unitofWork.save();
                obj.ProductFarmer.FarmerId = HttpContext.Session.GetString("UserId");

                obj.ProductFarmer.ProductId = obj.Product.ProductId;
                unitofWork.productFarmer.Add(obj.ProductFarmer);
                unitofWork.save();
                return RedirectToAction("Product", "Farmer");
            }
            return View();
        }

        [HttpGet]
        public IActionResult EditProduct(int? id)
        {

            if (id == null)
            {
                return View();
            }
            ProductVM productVM = new()
            {
                CategoryList = unitofWork.category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.CategoryName,
                    Value = u.CategoryId.ToString()
                }),
            };
            productVM.Product = unitofWork.product.Get(x => x.ProductId == id);
            return View(productVM);

        }

        [HttpPost]
        public IActionResult EditProduct(ProductVM obj, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwroothpath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productpath = Path.Combine(wwwroothpath, @"images\product");
                    if (!string.IsNullOrEmpty(obj.Product.Image))
                    {
                        var OldImagePath = Path.Combine(wwwroothpath, obj.Product.Image.TrimStart('\\'));

                        if (System.IO.File.Exists(OldImagePath))
                        {
                            System.IO.File.Delete(OldImagePath);
                        }
                    }
                    using (var fileStream = new FileStream(Path.Combine(productpath, filename), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    obj.Product.Image = @"\images\product\" + filename;
                }
                if(obj.ProductFarmer == null)
                {
                    obj.ProductFarmer = new ProductFarmer();
                }
                unitofWork.product.Update(obj.Product);
                unitofWork.save();
                return RedirectToAction("Product", "Farmer");
            }
            return View();

        }

        [HttpGet]
        public IActionResult DeleteProduct(int? id)
        {
            if (id == null)
            {
                return View();
            }
            ProductVM productVM = new()
            {
                CategoryList = unitofWork.category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.CategoryName,
                    Value = u.CategoryId.ToString()
                }),
            };
            productVM.Product = unitofWork.product.Get(x => x.ProductId == id);
            return View(productVM);
        }

        [HttpPost]
        public IActionResult DeleteProduct(ProductVM obj, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwroothpath = _webHostEnvironment.WebRootPath;
                if (!string.IsNullOrEmpty(obj.Product.Image))
                {
                    var OldImagePath = Path.Combine(wwwroothpath, obj.Product.Image.TrimStart('\\'));

                    if (System.IO.File.Exists(OldImagePath))
                    {
                        System.IO.File.Delete(OldImagePath);
                    }
                }

                if(obj.ProductFarmer == null)
                {
                    obj.ProductFarmer = new ProductFarmer();
                }

                obj.ProductFarmer.FarmerId = HttpContext.Session.GetString("UserId");
                obj.ProductFarmer.ProductId = obj.Product.ProductId;

                // Retrieve the ProductFarmer entity from the database
                var productFarmer = unitofWork.productFarmer.GetFirstOrDefault(pf => pf.FarmerId == obj.ProductFarmer.FarmerId && pf.ProductId == obj.ProductFarmer.ProductId);
                if (productFarmer != null)
                {
                    unitofWork.productFarmer.Delete(productFarmer);
                }

                unitofWork.product.Delete(obj.Product);
                unitofWork.save();

                return RedirectToAction("Product", "Farmer");
            }
            return View();
        }

    }
}
