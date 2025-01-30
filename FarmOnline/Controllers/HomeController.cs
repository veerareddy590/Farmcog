using FarmOnline.Models;
using FarmOnline.Repositories.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FarmOnline.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public IUnitofWork _unitofWork;
        public HomeController(ILogger<HomeController> logger,IUnitofWork unitofWork)
        {
            _logger = logger;
            _unitofWork = unitofWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> productList = _unitofWork.product.Getproducts(includeProperties: "Category");
            return View(productList);
        }

        //public IActionResult Privacy()
        //{
        //    return View();
        //}

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}
