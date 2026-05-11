using Microsoft.AspNetCore.Mvc;
using TransportManagementSG.Application.Interfaces.Repository;
using TransportManagementSG.Application.Services;
using TransportManagementSG.UI.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace TransportManagementSG.UI.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserService _UserService;
        private readonly IJwtService _jwtService;
        public LoginController(IUserService UserService, IJwtService jwtService)
        {
            _UserService = UserService;
            _jwtService = jwtService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View("Login");
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel model, CancellationToken token)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _UserService.ValidateUserAsync(model.Email, model.Password, token);

            if (user != null)
            {
                HttpContext.Session.SetString("UserEmail", user.Email);
                var jwtToken = _jwtService.GenerateToken(user.Email, user.Role);
                HttpContext.Session.SetString("JWToken", jwtToken);

                // Create claims for cookie authentication
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.Role, user.Role)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60)
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Invalid email or password");
            return View("Login", model);
        }

        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Remove("UserEmail");
            HttpContext.Session.Remove("JWToken");

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Login");
        }
    }


}
