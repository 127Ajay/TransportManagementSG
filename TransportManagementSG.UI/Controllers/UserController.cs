using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TransportManagementSG.Application.Interfaces.Repository;
using TransportManagementSG.Application.Repository;
using TransportManagementSG.Application.Services;
using TransportManagementSG.Contracts.Model;
using TransportManagementSG.UI.ViewModels;

namespace TransportManagementSG.UI.Controllers
{
    //[Authorize]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _UserService;
        private readonly IRoleService _roleService;
        public UserController(IUserService Userervice, IRoleService RoleService)
        {
            _UserService = Userervice;
            _roleService = RoleService;
        }

       
        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var roles = await _roleService.GetAllRoles(cancellationToken);

            var model = new UserViewModel
            {
                Roles = roles.Select(r => new SelectListItem
                {
                    Value = r.RoleId.ToString(),
                    Text = r.RoleName
                }).ToList()
            };

            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> GetAllUsers(CancellationToken token)
        {
            var username = User.Identity.Name;
            var users = await _UserService.GetAllUsersAsync(token);
            return Json(new
            {
                LoggedInUser = username,
                Data = users
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRoles(CancellationToken cancellationToken)
        {
            var roles = await _roleService.GetAllRoles(cancellationToken);

            var result = roles.Select(r => new
            {
                roleID = r.RoleId,
                roleName = r.RoleName
            });

            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserViewModel model, CancellationToken token)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                RoleID = (int)model.RoleID
            };


            await _UserService.AddUser(user, token);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(int id, CancellationToken token)
        {
            await _UserService.DeleteUserAsync(id, token);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetUserById(int id, CancellationToken token)
        {
            var user = await _UserService.GetUserByIdAsync(id, token);
            return Json(user);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser([FromBody] UserViewModel model, CancellationToken token)
        {
            var user = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                RoleID = (int)model.RoleID,
                UserId = (int)model.UserId
                
            };

            await _UserService.UpdateUserAsync(user, token);
            return Ok();
        }
    }
}   
    