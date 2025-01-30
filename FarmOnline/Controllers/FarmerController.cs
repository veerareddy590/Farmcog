
using FarmOnline.Models;
using FarmOnline.Models.ViewModel;
using FarmOnline.Repositories;
using FarmOnline.Repositories.IRepository;
using Microsoft.AspNetCore.Authorization; 
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace FarmOnline.Controllers
{
    [Authorize(Roles ="Farmer")]
    public class FarmerController : Controller
    {
        public IUnitofWork unitofWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl = "https://localhost:7088/api/Categories";

        public FarmerController(IUnitofWork unitofWork, IWebHostEnvironment _webHostEnvironment, UserManager<IdentityUser> userManager)
        {
            this.unitofWork = unitofWork;
            this._webHostEnvironment = _webHostEnvironment;
            _userManager = userManager;
            _httpClient = new HttpClient();
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User) as ApplicationUser;
            if (user == null)
            {
                return NotFound();
            }
            var farmerId = user.Id;

            var totalproducts =  unitofWork.productFarmer.GatAll(u=>u.FarmerId==farmerId).Count();

            var totalOrders = unitofWork.orderHeader.GetAllWithDetails()
                .Where(o => o.OrderDetails.Any(od => od.Product.productFarmers.Any(pf => pf.FarmerId == farmerId))).Count();


            var totalOrdersShipped = unitofWork.orderHeader.GetAllWithDetails()
                .Where(o => o.OrderStatus == "Shipped" && o.OrderDetails.Any(od => od.Product.productFarmers.Any(pf => pf.FarmerId == farmerId))).Count();

            var totalOrdersPending = unitofWork.orderHeader.GetAllWithDetails()
                .Where(o => o.OrderStatus == "Pending" && o.OrderDetails.Any(od => od.Product.productFarmers.Any(pf => pf.FarmerId == farmerId))).Count();

            var totalOrdersPlaced = unitofWork.orderHeader.GetAllWithDetails()
                .Where(o => o.OrderStatus == "PlacedOrder" && o.OrderDetails.Any(od => od.Product.productFarmers.Any(pf => pf.FarmerId == farmerId))).Count();

            var dashboardVM = new FarmerDashVM
            {
                TotalProducts = totalproducts,
                TotalOrders = totalOrders,
                TotalOrdersShipped = totalOrdersShipped,
                TotalOrdersPending = totalOrdersPending,
                TotalOrdersPlaced = totalOrdersPlaced
            };

            return View(dashboardVM);


            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Product()
        {
            

            var user = await _userManager.GetUserAsync(User) as ApplicationUser;
            var farmerId = user.Id;
            var allproducts = unitofWork.product.GetAll();
            var prodcutid = unitofWork.productFarmer.GetAll().Where(x => x.FarmerId == farmerId);
            var products = (from product in allproducts
                            join productids in prodcutid on product.ProductId equals productids.ProductId
                            select product).ToList();

            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> UploadProduct()
        {
            var response = await _httpClient.GetAsync(_apiBaseUrl);
            IEnumerable<Category> categorieslist = new List<Category>();

            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                categorieslist = JsonConvert.DeserializeObject<IEnumerable<Category>>(jsonData);
            }
            if (categorieslist == null)
            {

                return View("Error");
            }

            ProductVM productVM = new()
            {
                CategoryList = categorieslist.Select(u => new SelectListItem
                {
                    Text = (string)u.CategoryName,
                    Value = u.CategoryId.ToString()
                }),
                Product = new Product()
            };

            return View(productVM);
        }

        [HttpPost]
        public async Task<IActionResult> UploadProduct(ProductVM obj, IFormFile? file)
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
                var user = await _userManager.GetUserAsync(User) as ApplicationUser;
                obj.ProductFarmer.FarmerId = user.Id;

                obj.ProductFarmer.ProductId = obj.Product.ProductId;
                unitofWork.productFarmer.Add(obj.ProductFarmer);
                unitofWork.save();
                return RedirectToAction("Product", "Farmer");
            }
            var response = await _httpClient.GetAsync(_apiBaseUrl);
            IEnumerable<Category> categorieslist = new List<Category>();

            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                categorieslist = JsonConvert.DeserializeObject<IEnumerable<Category>>(jsonData);
            }
            if (categorieslist == null)
            {

                return View("Error");
            }
            obj.CategoryList = categorieslist.Select(u => new SelectListItem
            {
                Text = (string)u.CategoryName,
                Value = u.CategoryId.ToString()
            });
            return View(obj);
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
        public async Task<IActionResult> DeleteProduct(ProductVM obj, IFormFile? file)
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
                var user = await _userManager.GetUserAsync(User) as ApplicationUser;

                obj.ProductFarmer.FarmerId = user.Id;
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
