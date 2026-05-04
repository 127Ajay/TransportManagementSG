using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using TransportManagementSG.Application.Interfaces.Repository;
using TransportManagementSG.UI.Extensions;
using TransportManagementSG.UI.ViewModels;

namespace TransportManagementSG.UI.Controllers;

public class RoleController : Controller
{
    private readonly IRoleService _roleService;
    private readonly ILogger<RoleController> _logger;
    public RoleController(IRoleService roleService, ILogger<RoleController> logger)
    {
        _roleService = roleService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken = default)
    {
        var roles = await _roleService.GetAllRoles(cancellationToken);
        var viewModel = roles.Select(r => r.ToViewModel()).ToList();
        return View(viewModel);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new RoleViewModel { IsActive = true });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(RoleViewModel model, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
            return View(model);
        
        var existingRole = await _roleService.GetRoleByName(model.RoleName, cancellationToken);

        if (existingRole != null)
        {
            ModelState.AddModelError(nameof(model.RoleName), "Role already exists.");
            return View(model);
        }
        
        var role = model.ToModel();
        await _roleService.CreateRole(role, cancellationToken);

        TempData["Success"] = "Role created successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken = default)
    {
        var role = await _roleService.GetRoleById(id, cancellationToken);
        if (role == null)
            return NotFound();

        return View(role.ToViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(RoleViewModel model, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
            return View(model);

        
        var existingRole = await _roleService.GetRoleById(model.RoleId, cancellationToken);

        // 🔴 Rule 1: Prevent saving if name is unchanged
        if (string.Equals(existingRole.RoleName, model.RoleName, StringComparison.OrdinalIgnoreCase))
        {
            ModelState.AddModelError(nameof(model.RoleName), "No changes detected in Role Name.");
            return View(model);
        }

        // 🔴 Rule 2: Prevent duplicates (excluding current record)
        var duplicate = await _roleService.GetRoleByName(model.RoleName, cancellationToken);

        if (duplicate != null && duplicate.RoleId != model.RoleId)
        {
            ModelState.AddModelError(nameof(model.RoleName), "Role already exists.");
            return View(model);
        }
        
        var role = model.ToModel();
        var updated = await _roleService.UpdateRole(role, cancellationToken);
        if (!updated)
        {
            ModelState.AddModelError(string.Empty, "Unable to update the role. Please try again.");
            return View(model);
        }

        TempData["Success"] = "Role updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    // POST: /Role/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
    {
        var deleted = await _roleService.DeleteRole(id, cancellationToken);
        if (!deleted)
            TempData["Error"] = "Role could not be deleted.";
        else
            TempData["Success"] = "Role deleted.";

        return RedirectToAction(nameof(Index));
    }
}