using FarmOnline.Models;
using FarmOnline.Models.ViewModel;
using FarmOnline.Repositories;
using FarmOnline.Repositories.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FarmOnline.Controllers
{
    [Authorize(Roles = "Customer")]
    public class CustomerController : Controller
    {
        public IUnitofWork unitofWork;
        private readonly UserManager<IdentityUser> _userManager;

        public CustomerController(IUnitofWork unitofWork, UserManager<IdentityUser> userManager)
        {
            this.unitofWork = unitofWork;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            IEnumerable<Product> productList = unitofWork.product.Getproducts(includeProperties:"Category");
            return View(productList);
        }
        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId)
        {
            var user = await _userManager.GetUserAsync(User) as ApplicationUser;
            var userId = user.Id;
            var cartItem = unitofWork.Cart.GetFirstOrDefault(c => c.ProductId == productId && c.CustomerId == userId);
            if (cartItem == null)
            {
                cartItem = new Cart
                {
                    ProductId = productId,
                    CustomerId = userId,
                };
                unitofWork.Cart.Add(cartItem);
            }
            else
            {
                cartItem.count += 1;
            }
            unitofWork.save();
            return RedirectToAction("Cart","Customer");
        }

        public async Task<IActionResult> Cart()
        {
            var user = await _userManager.GetUserAsync(User) as ApplicationUser;
            var userId = user.Id;
            var cartItems = unitofWork.Cart.Getcart(c => c.CustomerId == userId, includeProperties: "Product");
            return View(cartItems);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCart(int productId, string action)
        {
            var user = await _userManager.GetUserAsync(User) as ApplicationUser;
            var userId = user.Id;
            var cartItem = unitofWork.Cart.GetFirstOrDefault(c => c.ProductId == productId && c.CustomerId == userId);
            if (cartItem != null)
            {
                if (action == "increase")
                {
                    cartItem.count += 1;
                }
                else if (action == "decrease" && cartItem.count > 1)
                {
                    cartItem.count -= 1;
                }
                unitofWork.save();
            }
            return RedirectToAction("Cart");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            var user = await _userManager.GetUserAsync(User) as ApplicationUser;
            var userId = user.Id;
            var cartItem = unitofWork.Cart.GetFirstOrDefault(c => c.ProductId == productId && c.CustomerId == userId);
            if (cartItem != null)
            {
                unitofWork.Cart.Delete(cartItem);
                unitofWork.save();
            }
            return RedirectToAction("Cart");
        }

        public async Task<IActionResult> Address()
        {
            var user = await _userManager.GetUserAsync(User) as ApplicationUser;
            var userId = user.Id;
            var address= unitofWork.address.GetFirstOrDefault(u=>u.UserId == userId);
            if(address != null)
            {
                return View(address);
            }
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> Address(Address Address) {
            var user = await _userManager.GetUserAsync(User) as ApplicationUser;
            var userId = user.Id;
            var addresslist = unitofWork.address.GetFirstOrDefault(u => u.UserId == userId);
            if (addresslist == null) {
                Address.UserId = userId;
                unitofWork.address.Add(Address);
                unitofWork.save();
                return RedirectToAction("Summary","Customer");
            }
            else
            {
                
                addresslist.StreetAddress = Address.StreetAddress;
                addresslist.State = Address.State;
                addresslist.City = Address.City;
                addresslist.PostalCode = Address.PostalCode;
                unitofWork.address.update(addresslist);
                unitofWork.save();
                return RedirectToAction("Summary","Customer");
            }
            

        }


        public async Task<IActionResult> Summary()
        {
            var user = await _userManager.GetUserAsync(User) as ApplicationUser;
            var userId = user.Id;
            var address = unitofWork.address.GetFirstOrDefault(u => u.UserId == userId);
            var cartItems = unitofWork.Cart.Getcart(c => c.CustomerId == userId, includeProperties: "Product");

            var summaryViewModel = new SummaryVM
            {
                Address = address,
                CartItems = cartItems,
                TotalAmount = cartItems.Sum(item => item.count * item.Product.Price)
            };

            return View(summaryViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder()
        {
            var user = await _userManager.GetUserAsync(User) as ApplicationUser;
            var userId = user.Id;
            var cartItems = unitofWork.Cart.Getcart(c => c.CustomerId == userId, includeProperties: "Product");
            var address = unitofWork.address.GetFirstOrDefault(u => u.UserId == userId);

            if (cartItems == null || !cartItems.Any() || address == null)
            {
                // Handle the case where the cart is empty or address is missing
                return RedirectToAction("Summary");
            }

            var orderHeader = new OrderHeader
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                ShippingDate = DateTime.Now.AddDays(3), // Example shipping date
                OrderTotal = cartItems.Sum(item => item.count * item.Product.Price),
                OrderStatus = "PlacedOrder", // Example status
                PaymentStatus = 1 // Example payment status
            };

            unitofWork.orderHeader.Add(orderHeader);
            unitofWork.save();

            foreach (var item in cartItems)
            {
                var orderDetail = new OrderDetail
                {
                    OrderId = orderHeader.Id,
                    ProductId = item.ProductId,
                    Count = item.count,
                    Price = item.Product.Price
                };
                unitofWork.orderDetail.Add(orderDetail);
            }

            unitofWork.save();

            // Clear the cart
            unitofWork.Cart.RemoveRange(cartItems);
            unitofWork.save();

            return RedirectToAction("OrderConformation", new { orderId = orderHeader.Id });
        }

        public IActionResult OrderConformation(int orderId)
        {
            var orderHeader = unitofWork.orderHeader.GetFirstOrDefault(o => o.Id == orderId);
            if (orderHeader == null)
            {
                return NotFound();
            }

            var orderDetails = unitofWork.orderDetail.Getorders(d => d.OrderId == orderId, includeProperties: "Product");

            var orderConfirmationViewModel = new OrderComfVM
            {
                OrderHeader = orderHeader,
                OrderDetails = orderDetails
            };

            return View(orderConfirmationViewModel);
        }
    }
}

