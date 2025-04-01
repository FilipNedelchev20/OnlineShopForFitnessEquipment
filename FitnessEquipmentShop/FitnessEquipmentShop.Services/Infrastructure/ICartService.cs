using FitnessEquipmentShop.Data.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FitnessEquipmentShop.Services
{
    public interface ICartService
    {
        Task<IEnumerable<CartItem>> GetCartItemsAsync();
        Task AddToCartAsync(int productId, string userId);
        Task RemoveFromCartAsync(int cartItemId);
    }
}