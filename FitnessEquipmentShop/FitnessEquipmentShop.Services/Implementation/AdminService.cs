using FitnessEquipmentShop.Data.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FitnessEquipmentShop.Services
{
    public class AdminService : IAdminService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<List<string>> GetAllRolesAsync()
        {
            return await _roleManager.Roles.Select(r => r.Name).ToListAsync();
        }

        public async Task<bool> AssignRoleByIdAsync(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }

            await _userManager.AddToRoleAsync(user, role);
            return true;
        }

    }
}
