using FitnessEquipmentShop.Data.Models.Entities;
using System.Threading.Tasks;

namespace FitnessEquipmentShop.Services
{
    public interface IAdminService
    {
        Task<List<User>> GetAllUsersAsync();
        Task<List<string>> GetAllRolesAsync();
        Task<bool> AssignRoleByIdAsync(string userId, string role);
    }
}
