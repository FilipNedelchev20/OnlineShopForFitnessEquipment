using FitnessEquipmentShop.Data.Models.Entities;
using FitnessEquipmentShop.Web.ViewModel;
using System.Threading.Tasks;

namespace FitnessEquipmentShop.Services
{
    public interface IOrderService
    {
        Task<CheckoutViewModel> GetCheckoutDataAsync(string userId);
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId);
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task UpdateOrderStatusAsync(int orderId, string status);
        Task PlaceOrderAsync(string userId, Address address);
    }
}
