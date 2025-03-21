using FitnessEquipmentShop.Data.Models.Entities;
using Microsoft.AspNetCore.Identity;
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

        public async Task<bool> AssignRoleAsync(string email, string role)
        {
            var user = await _userManager.FindByEmailAsync(email);
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
