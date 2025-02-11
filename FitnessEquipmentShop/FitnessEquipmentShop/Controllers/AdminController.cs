using FitnessEquipmentShop.Data.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[Authorize(Roles = "Admin")] 
public class AdminController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AdminController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public IActionResult AssignRole()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AssignRole(string email, string role)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user != null)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }
            await _userManager.AddToRoleAsync(user, role);
            ViewBag.Message = $"Role {role} assigned to {email}!";
        }
        return View();
    }
}
