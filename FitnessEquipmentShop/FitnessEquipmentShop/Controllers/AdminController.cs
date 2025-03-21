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

    public IActionResult AssignRole()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AssignRole(string email, string role)
    {
        var success = await _adminService.AssignRoleAsync(email, role);
        ViewBag.Message = success ? $"Role {role} assigned to {email}!" : "User not found.";
        return View();
    }
}
