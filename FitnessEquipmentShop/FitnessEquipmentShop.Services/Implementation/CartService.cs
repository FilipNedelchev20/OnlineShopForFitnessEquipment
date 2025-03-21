using FitnessEquipmentShop.Data;
using FitnessEquipmentShop.Data.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessEquipmentShop.Services
{
    public class CartService : ICartService
    {
        private readonly ApplicationDbContext _context;
        public CartService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CartItem>> GetCartItemsAsync()
        {
            return await _context.CartItems.Include(c => c.Product).ToListAsync();
        }

        public async Task AddToCartAsync(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) return;
            var cartItem = new CartItem { ProductId = productId };
            await _context.CartItems.AddAsync(cartItem);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveFromCartAsync(int cartItemId)
        {
            var cartItem = await _context.CartItems.FindAsync(cartItemId);
            if (cartItem != null)
            {
                _context.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();
            }
        }
    }
}
