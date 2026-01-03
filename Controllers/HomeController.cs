using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SignalClientWeb.Models;
using Google.Apis.Auth;

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

        public class TokenRequest
        {
            public required string IdToken { get; set; }
        }

        public async Task<IActionResult> SigninGoogle([FromBody] TokenRequest request)
        {
            try
            {
                var payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken);

                // Create and return tokens or session
                var tokenProvider = new Users.Infrastructure.TokenProvider(_configuration);
                var accessToken = tokenProvider.Create(new Users.User
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = payload.Email,
                    EmailVerified = payload.EmailVerified
                });


                var result = Ok(new {
                    Message = $"Identity confirmed: {payload.Email}",
                    AccessToken = accessToken,
                    RefreshToken = tokenProvider.GenerateRefreshToken()
                });

                return result;
            }
            catch (Exception ex)
            {
                return Unauthorized(new { Message = "Invalid ID token", Error = ex.Message });
            }
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
