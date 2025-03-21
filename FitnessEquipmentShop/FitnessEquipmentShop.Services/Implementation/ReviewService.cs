using FitnessEquipmentShop.Data;
using FitnessEquipmentShop.Data.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessEquipmentShop.Services
{
    public class ReviewService : IReviewService
    {
        private readonly ApplicationDbContext _context;
        public ReviewService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Review>> GetReviewsByProductIdAsync(int productId)
        {
            return await _context.Reviews.Where(r => r.ProductId == productId).ToListAsync();
        }

        public async Task AddReviewAsync(Review review)
        {
            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();
        }
    }
}
