using FitnessEquipmentShop.Data.Models.Entities;
using FitnessEquipmentShop.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FitnessEquipmentShop.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly IOrderService _orderService;
        private readonly UserManager<User> _userManager;

        public AdminController(IAdminService adminService, UserManager<User> userManager = null, IOrderService orderService = null)
        {
            _adminService = adminService;
            _userManager = userManager;
            _orderService = orderService;
        }
        public async Task<IActionResult> Index()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return View(orders);
        }

        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            await _orderService.UpdateOrderStatusAsync(id, status);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> ManageUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            var model = users.Select(user => new ManageUserViewModel
            {
                Email = user.Email,
                Roles = _userManager.GetRolesAsync(user).Result.ToList(),
                RegisteredOn = user.RegistrationDate,  
                IsLockedOut = user.LockoutEnd > DateTime.UtcNow
            }).ToList();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AssignRole(string email, string role)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null && !string.IsNullOrEmpty(role))
            {
                var result = await _userManager.AddToRoleAsync(user, role);
                if (result.Succeeded)
                {
                    TempData["Message"] = $"Role '{role}' assigned to {email}.";
                }
                else
                {
                    TempData["Message"] = $"Failed to assign role.";
                }
            }
            return RedirectToAction("ManageUsers");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveAllRoles(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                TempData["Message"] = "User not found.";
                return RedirectToAction("ManageUsers");
            }

            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Any())
            {
                var result = await _userManager.RemoveFromRolesAsync(user, roles);
                if (result.Succeeded)
                {
                    TempData["Message"] = $"All roles removed from {email}.";
                }
                else
                {
                    TempData["Message"] = "Failed to remove roles.";
                }
            }
            else
            {
                TempData["Message"] = "User has no roles.";
            }

            return RedirectToAction("ManageUsers");
        }


        [HttpPost]
        public IActionResult LockUser(string email, int lockDuration)
        {
            var user = _userManager.FindByEmailAsync(email).Result;

            if (user != null)
            {
                // Lock the user for the specified duration
                var lockoutEnd = DateTime.UtcNow.AddMinutes(lockDuration);
                var result = _userManager.SetLockoutEndDateAsync(user, lockoutEnd).Result;

                if (result.Succeeded)
                {
                    ViewBag.Message = $"User {email} is locked until {lockoutEnd}.";
                }
                else
                {
                    ViewBag.Message = "Error occurred while locking the user.";
                }
            }

            return RedirectToAction("ManageUsers");
        }

        [HttpPost]
        public async Task<IActionResult> UnlockUser(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var result = await _userManager.SetLockoutEndDateAsync(user, null);
                if (result.Succeeded)
                {
                    TempData["Message"] = $"User {email} has been unlocked.";
                }
                else
                {
                    TempData["Message"] = $"Error while unlocking.";
                }
            }
            return RedirectToAction("ManageUsers");
        }
    }
}
