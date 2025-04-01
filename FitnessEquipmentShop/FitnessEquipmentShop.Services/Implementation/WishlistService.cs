using FitnessEquipmentShop.Data;
using FitnessEquipmentShop.Data.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessEquipmentShop.Services
{
    public class WishlistService : IWishlistService
    {
        private readonly ApplicationDbContext _context;
        public WishlistService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Wishlist>> GetWishlistItemsAsync()
        {
            return await _context.Wishlists.Include(w => w.Product).ToListAsync();
        }

        public async Task AddToWishlistAsync(int productId, string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new InvalidOperationException("User must be logged in to add items to the wishlist.");
            }

            var product = await _context.Products.FindAsync(productId);
            if (product == null) return;

            var wishlistItem = new Wishlist
            {
                UserId = userId,
                ProductId = productId
            };

            await _context.Wishlists.AddAsync(wishlistItem);
            await _context.SaveChangesAsync();
        }


        public async Task RemoveFromWishlistAsync(int wishlistId)
        {
            var wishlistItem = await _context.Wishlists.FindAsync(wishlistId);
            if (wishlistItem != null)
            {
                _context.Wishlists.Remove(wishlistItem);
                await _context.SaveChangesAsync();
            }
        }
    }
}