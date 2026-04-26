using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TransportManagementSG.Application.Interfaces.Repository;
using TransportManagementSG.UI.ViewModels;

namespace TransportManagementSG.UI.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IRoleService _RoleService;
        public UserController(IRoleService RoleService)
        {
            _RoleService = RoleService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Registration(CancellationToken token = default)
        {            
            UserRegistrationViewModel model = new UserRegistrationViewModel();
            
            var Roles = await _RoleService.GetAllRoles(token);
            model.Roles = Roles.Select(r => new SelectListItem
            {
                Value = r.RoleId.ToString(),
                Text = r.RoleName
            }).ToList();

            return View(model);
        }
    }
}
