using FitnessEquipmentShop.Data;
using FitnessEquipmentShop.Data.Models.Entities;
using FitnessEquipmentShop.Web.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

public class ProductController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProductController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var products = _context.Products.ToList();
        return View(products);
    }

    public async Task<IActionResult> Details(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return NotFound();
        return View(product);
    }
    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");
        return View();
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(CreateProductViewModel product)
    {
        if (ModelState.IsValid)
        {

            Product newProduct = new Product()
            {
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            CategoryId = product.CategoryId,
            StockQuantity = product.StockQuantity,
            ImageUrl = product.ImageUrl
            };
          await _context.Products.AddAsync(newProduct);
           await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name"); 
        return View(product);
    }

}
