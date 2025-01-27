using FarmOnline.Models;
using FarmOnline.Repositories.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace FarmOnline.Controllers
{
    public class UserController : Controller
    {
        public IUnitofWork unitofWork;
        public UserController(IUnitofWork unitofWork) { 
            this.unitofWork = unitofWork;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User obj)
        {
            if (ModelState.IsValid)
            {
                unitofWork.user.Add(obj);
                int r = unitofWork.save();
                if (r > 0)
                {

                    return RedirectToAction("Index", "Home");

                }
            }
                
            return View(obj);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string? UserId,string? Password)
        {

            User tempuser1 = unitofWork.user.searchLogin(UserId);
            if (tempuser1 == null)
            {
                TempData["Login"] = "Login Invalid";
                return NotFound();
            }
            else
            {
                HttpContext.Session.SetString("UserId",UserId);
                if (tempuser1.Password == Password && tempuser1.Roles == "Farmer")
                {

                    return RedirectToAction("Index", "Farmer");
                }
                else if (tempuser1.Password == Password && tempuser1.Roles == "Customer")
                {
                    return RedirectToAction("Index", "Customer");
                }
                else
                {
                    return NotFound();
                }
            }
            return View();

        }



        
    }
}
