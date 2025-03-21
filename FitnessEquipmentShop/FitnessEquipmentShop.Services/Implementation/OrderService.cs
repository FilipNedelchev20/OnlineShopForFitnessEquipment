using FitnessEquipmentShop.Data;
using FitnessEquipmentShop.Data.Models.Entities;
using FitnessEquipmentShop.Web.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessEquipmentShop.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;
        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CheckoutViewModel> GetCheckoutDataAsync(string userId)
        {
            var cartItems = await _context.CartItems
                .Include(c => c.Product)
                .Where(c => c.UserId == userId)
                .ToListAsync();

            if (!cartItems.Any())
            {
                return null;
            }

            var checkoutViewModel = new CheckoutViewModel
            {
                CartItems = cartItems,
                TotalPrice = cartItems.Sum(c => c.Product.Price * c.Quantity),
                Address = await _context.Addresses.FirstOrDefaultAsync(a => a.UserId == userId)
            };

            return checkoutViewModel;
        }

        public async Task PlaceOrderAsync(string userId, Address address)
        {
            var cartItems = await _context.CartItems
                .Include(c => c.Product)
                .Where(c => c.UserId == userId)
                .ToListAsync();

            if (!cartItems.Any())
            {
                throw new Exception("Cart is empty");
            }

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                TotalAmount = cartItems.Sum(c => c.Product.Price * c.Quantity),
                OrderDetails = cartItems.Select(c => new OrderDetail
                {
                    ProductId = c.ProductId,
                    Quantity = c.Quantity,
                    UnitPrice = c.Product.Price
                }).ToList(),
                Address = address
            };

            await _context.Orders.AddAsync(order);
            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
        }
    }
}
