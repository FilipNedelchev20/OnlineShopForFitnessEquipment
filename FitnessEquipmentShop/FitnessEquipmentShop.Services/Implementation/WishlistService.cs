using FitnessEquipmentShop.Data.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FitnessEquipmentShop.Services
{
    public interface IWishlistService
    {
        Task<IEnumerable<Wishlist>> GetWishlistItemsAsync();
        Task AddToWishlistAsync(int productId);
        Task RemoveFromWishlistAsync(int wishlistId);
    }
}
