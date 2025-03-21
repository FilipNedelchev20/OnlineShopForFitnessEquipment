using FitnessEquipmentShop.Services;
using FitnessEquipmentShop.Web.ViewModel.Home;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessEquipmentShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;

        public HomeController(ILogger<HomeController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllProductsAsync();
            var topProducts = products.OrderByDescending(p => p.Rating)
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
