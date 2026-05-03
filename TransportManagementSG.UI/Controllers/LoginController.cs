using Microsoft.AspNetCore.Mvc;
using TransportManagementSG.Application.Interfaces.Repository;
using TransportManagementSG.Application.Services;
using TransportManagementSG.UI.ViewModels;

namespace TransportManagementSG.UI.Controllers
{   
    public class LoginController : Controller
    {
        private readonly IUserService _UserService;
        public LoginController(IUserService UserService)
        {
            _UserService = UserService;
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
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Invalid email or password");
            return View(model);
        }
    }


}
