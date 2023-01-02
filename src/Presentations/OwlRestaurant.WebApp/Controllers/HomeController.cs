using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace OwlRestaurant.WebApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}