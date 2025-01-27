using FarmOnline.Models.ViewModel;
using FarmOnline.Repositories;
using FarmOnline.Repositories.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace FarmOnline.Controllers
{
    public class OrderController : Controller
    {
        public IUnitofWork unitofWork;

        public OrderController(IUnitofWork unitofWork)
        {
            this.unitofWork = unitofWork;
        }

        public IActionResult Order()
        {
            var userId = HttpContext.Session.GetString("UserId");
            var orders = unitofWork.orderHeader.Getorderheader(o => o.UserId == userId, includeProperties: "OrderDetails,OrderDetails.Product");

            var orderViewModels = orders.Select(order => new OrderComfVM
            {
                OrderHeader = order,
                OrderDetails = order.OrderDetails
            }).ToList();

            return View(orderViewModels);
        }

        public IActionResult OrderFarmer()
        {
            var farmerId = HttpContext.Session.GetString("UserId");
            var orders = unitofWork.orderHeader.GetAllWithDetails()
                .Where(o => o.OrderDetails.Any(od => od.Product.productFarmers.Any(pf => pf.FarmerId == farmerId)))
                .Select(o => new OrderComfVM
                {
                    OrderHeader = o,
                    OrderDetails = o.OrderDetails
                }).ToList();

            return View(orders);
        }
    }
}
