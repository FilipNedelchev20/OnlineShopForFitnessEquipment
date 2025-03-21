using FitnessEquipmentShop.Data.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FitnessEquipmentShop.Services
{
    public interface IReviewService
    {
        Task<IEnumerable<Review>> GetReviewsByProductIdAsync(int productId);
        Task AddReviewAsync(Review review);
    }
}
