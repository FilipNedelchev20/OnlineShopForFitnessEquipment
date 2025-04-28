using FitnessEquipmentShop.Data.Models.Entities;
using System.Threading.Tasks;

namespace FitnessEquipmentShop.Services
{
    public interface IAdminService
    {
        Task<IEnumerable<UserViewModel>> GetAllUsersWithRolesAsync();
        Task<bool> AssignRoleAsync(string userId, string role);
        Task<bool> RemoveAllRolesAsync(string userId);
        Task<bool> LockUserAsync(string userId, int days = 1);
        Task<bool> UnlockUserAsync(string userId);
    }
}
