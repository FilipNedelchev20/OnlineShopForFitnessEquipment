using System.Threading.Tasks;

namespace FitnessEquipmentShop.Services
{
    public interface IAdminService
    {
        Task<bool> AssignRoleAsync(string email, string role);
    }
}
