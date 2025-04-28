using FitnessEquipmentShop.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    public async Task<IActionResult> ManageUsers()
    {
        var users = await _adminService.GetAllUsersAsync();
        var roles = await _adminService.GetAllRolesAsync();

        ViewBag.Roles = roles;
        return View(users);
    }

    [HttpPost]
    public async Task<IActionResult> AssignRole(string userId, string role)
    {
        var success = await _adminService.AssignRoleByIdAsync(userId, role);

        TempData["Message"] = success ? "Role assigned successfully!" : "Error assigning role.";
        return RedirectToAction(nameof(ManageUsers));
    }
}
