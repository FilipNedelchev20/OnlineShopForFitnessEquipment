using FitnessEquipmentShop.Services;
using FitnessEquipmentShop.Web.ViewModel.Home;
using FitnessEquipmentShop.Web.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace FitnessEquipmentShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IEmailSender _emailService;


        public HomeController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllProductsAsync();
            var categories = await _categoryService.GetAllCategoriesAsync();

            var topProducts = products
                .OrderByDescending(p => p.Rating)
                .Take(6)
                .Select(p => new ProductViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl
                })
                .ToList();

            var topCategories = categories
                .Select(c => new CategoryViewModel
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToList();

            var model = new HomeViewModel
            {
                TopProducts = topProducts,
                TopCategories = topCategories
            };

            return View(model);
        }
        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact(ContactFormModel model)
        {
            if (ModelState.IsValid)
            {
                var message = $"Name: {model.Name}\nEmail: {model.Email}\nSubject: {model.Subject}\nMessage: {model.Message}";
                //await _emailService.SendEmailAsync("your-email@example.com", "Contact Form Submission", message);
                TempData["MessageSent"] = "Thank you! Your message has been sent successfully.";
                return RedirectToAction("Contact");

            }
            return View(model);
        }

        //public IActionResult ContactConfirmation()
        //{
        //    return View();
        //}

    }
}
