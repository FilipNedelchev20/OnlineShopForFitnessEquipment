using System.Diagnostics;
using FitnessEquipmentShop.Data;
using FitnessEquipmentShop.Models;
using FitnessEquipmentShop.Web.ViewModel;
using FitnessEquipmentShop.Web.ViewModel.Home;
using Microsoft.AspNetCore.Mvc;

namespace FitnessEquipmentShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var topProducts = _context.Products
                .OrderByDescending(p => p.Rating)
                .Take(5)
                .Select(p => new ProductViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl
                })
                .ToList();

            var viewModel = new HomeViewModel { TopProducts = topProducts };

            return View(viewModel);
        }
    }
}
