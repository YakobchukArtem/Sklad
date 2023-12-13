using Microsoft.AspNetCore.Mvc;
using Sklad.Models;
using System.Diagnostics;

namespace Sklad.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SignIn(UserSignInDto user_sign_in)
        {
            var user = DataBase.get_user(user_sign_in.login);
            if (user!=null && user_sign_in.password == user.Password)
            {
                User_Name.Name = user.Name;
                return RedirectToAction("Products", "Products");
            }
            else
            {
                TempData["ErrorMessage"] = "Неправильний логін або пароль";
                return RedirectToAction("Index");
            }

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}