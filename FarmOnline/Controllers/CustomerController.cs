using FarmOnline.Models;
using FarmOnline.Models.ViewModel;
using FarmOnline.Repositories;
using FarmOnline.Repositories.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace FarmOnline.Controllers
{
    public class CustomerController : Controller
    {
        public IUnitofWork unitofWork;

        public CustomerController(IUnitofWork unitofWork)
        {
            this.unitofWork = unitofWork;
        }
        public IActionResult Index()
        {
            IEnumerable<Product> productList = unitofWork.product.Getproducts(includeProperties:"Category");
            return View(productList);
        }
        [HttpPost]
        public IActionResult AddToCart(int productId)
        {
            var userId = HttpContext.Session.GetString("UserId");
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

        public IActionResult Cart()
        {
            var userId = HttpContext.Session.GetString("UserId");
            var cartItems = unitofWork.Cart.Getcart(c => c.CustomerId == userId, includeProperties: "Product");
            return View(cartItems);
        }

        [HttpPost]
        public IActionResult UpdateCart(int productId, string action)
        {
            var userId = HttpContext.Session.GetString("UserId");
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
        public IActionResult RemoveFromCart(int productId)
        {
            var userId = HttpContext.Session.GetString("UserId");
            var cartItem = unitofWork.Cart.GetFirstOrDefault(c => c.ProductId == productId && c.CustomerId == userId);
            if (cartItem != null)
            {
                unitofWork.Cart.Delete(cartItem);
                unitofWork.save();
            }
            return RedirectToAction("Cart");
        }

        public IActionResult Address()
        {
            var userId = HttpContext.Session.GetString("UserId");
            var address= unitofWork.address.GetFirstOrDefault(u=>u.UserId == userId);
            if(address != null)
            {
                return View(address);
            }
            return View();

        }

        [HttpPost]
        public IActionResult Address(Address Address) {
            var userId = HttpContext.Session.GetString("UserId");
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


        public IActionResult Summary()
        {
            var userId = HttpContext.Session.GetString("UserId");
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
        public IActionResult PlaceOrder()
        {
            var userId = HttpContext.Session.GetString("UserId");
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
                OrderStatus = 1, // Example status
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

