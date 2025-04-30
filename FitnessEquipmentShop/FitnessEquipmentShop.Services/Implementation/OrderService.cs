using FitnessEquipmentShop.Data;
using FitnessEquipmentShop.Data.Models.Entities;
using FitnessEquipmentShop.Services.Infrastructure;
using FitnessEquipmentShop.Web.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessEquipmentShop.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;
        private readonly UserManager<User> _userManager;

        public OrderService(ApplicationDbContext context, IEmailService emailService, UserManager<User> userManager)
        {
            _context = context;
            _emailService = emailService;
            _userManager = userManager;
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

            return new CheckoutViewModel
            {
                CartItems = cartItems,
                TotalPrice = cartItems.Sum(c => c.Product.Price * c.Quantity),
                Address = await _context.Addresses.FirstOrDefaultAsync(a => a.UserId == userId)
            };
        }
        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId)
        {
            return await _context.Orders
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }
        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task UpdateOrderStatusAsync(int orderId, string status)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order != null)
            {
                order.Status = status;
                await _context.SaveChangesAsync();
            }
        }
        public async Task PlaceOrderAsync(string userId, Address inputAddress)
        {
            var cartItems = await _context.CartItems
                .Include(c => c.Product)
                .Where(c => c.UserId == userId)
                .ToListAsync();

            if (!cartItems.Any())
            {
                throw new Exception("Cart is empty");
            }

            // 1. Проверка за наличности
            foreach (var item in cartItems)
            {
                if (item.Product.StockQuantity < item.Quantity)
                {
                    throw new Exception($"Not enough stock for {item.Product.Name}. Available: {item.Product.StockQuantity}, Requested: {item.Quantity}");
                }
            }

            // 2. Взимане или създаване на адрес
            Address address = await _context.Addresses
                .FirstOrDefaultAsync(a => a.UserId == userId && a.Street == inputAddress.Street && a.City == inputAddress.City);

            if (address == null)
            {
                address = new Address
                {
                    UserId = userId,
                    Street = inputAddress.Street,
                    City = inputAddress.City
                };
                _context.Addresses.Add(address);
                await _context.SaveChangesAsync(); // За да имаме валидно AddressId
            }

            // 3. Създаване на Order
            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                AddressId = address.Id,
                TotalAmount = cartItems.Sum(c => c.Product.Price * c.Quantity),
                Status = "Processing"
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync(); // За да имаме Order.Id

            // 4. Добавяне на OrderDetails
            foreach (var item in cartItems)
            {
                var orderDetail = new OrderDetail
                {
                    OrderId = order.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.Product.Price
                };

                _context.OrderDetails.Add(orderDetail);

                // 5. Намаляване на количеството на продукта
                item.Product.StockQuantity -= item.Quantity;
                _context.Products.Update(item.Product);
            }

            // 6. Премахване от количката
            _context.CartItems.RemoveRange(cartItems);

            // 7. Запазване
            await _context.SaveChangesAsync();


            var user = await _userManager.FindByIdAsync(userId);
            var userEmail = user?.Email;

            if (!string.IsNullOrEmpty(userEmail))
            {
                await _emailService.SendEmailAsync(userEmail, "Order Confirmation", "Your order has been placed successfully.");
            }

        }

    }
}
