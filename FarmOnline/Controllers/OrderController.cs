using FarmOnline.Models;
using FarmOnline.Models.ViewModel;
using FarmOnline.Repositories;
using FarmOnline.Repositories.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FarmOnline.Controllers
{
    public class OrderController : Controller
    {
        public IUnitofWork unitofWork;
        private readonly UserManager<IdentityUser> _userManager;

        public OrderController(IUnitofWork unitofWork,UserManager<IdentityUser> userManager)
        {
            this.unitofWork = unitofWork;
            _userManager = userManager;

        }
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Order()
        {
            var user = await _userManager.GetUserAsync(User) as ApplicationUser;
            var userId = user.Id;
            var orders = unitofWork.orderHeader.Getorderheader(o => o.UserId == userId, includeProperties: "OrderDetails,OrderDetails.Product");

            var orderViewModels = orders.Select(order => new OrderComfVM
            {
                OrderHeader = order,
                OrderDetails = order.OrderDetails
            }).ToList();

            return View(orderViewModels);
        }
        //[Authorize(Roles = "Farmer")]
        //public async Task<IActionResult> OrderFarmer()
        //{
        //    var user = await _userManager.GetUserAsync(User) as ApplicationUser;

        //    var farmerId = user.Id;
        //    var orders = unitofWork.orderHeader.GetAllWithDetails()
        //        .Where(o => o.OrderDetails.Any(od => od.Product.productFarmers.Any(pf => pf.FarmerId == farmerId)))
        //        .Select(o => new OrderComfVM
        //        {
        //            OrderHeader = o,
        //            OrderDetails = o.OrderDetails
        //        }).ToList();

        //    return View(orders);
        //}

        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> OrderFarmer()
        {
            var user = await _userManager.GetUserAsync(User) as ApplicationUser;
            if (user == null)
            {
                return NotFound();
            }

            var farmerId = user.Id;
            var orders = unitofWork.orderHeader.GetAllWithDetails()
                .Where(o => o.OrderDetails.Any(od => od.Product.productFarmers.Any(pf => pf.FarmerId == farmerId)))
                .Select(o => new OrderComfVM
                {
                    OrderHeader = o,
                    OrderDetails = o.OrderDetails.Where(od => od.Product.productFarmers.Any(pf => pf.FarmerId == farmerId)).ToList()
                }).ToList();

            return View(orders);
        }

        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, string status)
        {
            var orderHeader = await unitofWork.orderHeader.GetFirstOrDefaultAsync(o => o.Id == orderId);
            if (orderHeader == null)
            {
                return NotFound();
            }

            orderHeader.OrderStatus = status;
            unitofWork.save();

            return RedirectToAction("OrderFarmer");
        }
    }
}
