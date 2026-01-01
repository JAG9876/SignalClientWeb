using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SignalClientWeb.Models;

namespace SignalClientWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public IActionResult Index(HomeViewModel model)
        {
            string clientId = _configuration["Authentication:Google:ClientId"] ?? "Not configured";
            model.GoogleClientId = clientId;
            return View(model);
        }

        public IActionResult SigninGoogle()
        {
            return View(); // Might have to specify the view
        }

        public IActionResult Privacy()
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
