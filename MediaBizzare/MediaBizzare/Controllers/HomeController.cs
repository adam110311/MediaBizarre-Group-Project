using System.Diagnostics;
using MediaBizzare.Models;
using Microsoft.AspNetCore.Mvc;

namespace MediaBizzare.Controllers
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

        public IActionResult HomePage()
        {
            return View();
        }
        public IActionResult Categories()
        {
            return View();
        }
        public IActionResult CompLap()
        {
            return View();
        }
        public IActionResult GameDivert()
        {
            return View();
        }
        public IActionResult HA()
        {
            return View();
        }
        public IActionResult PhoneWear()
        {
            return View();
        }
        public IActionResult TVaudio()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
