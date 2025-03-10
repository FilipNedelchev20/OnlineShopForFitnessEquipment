using FitnessEquipmentShop.Data;
using FitnessEquipmentShop.Data.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

public class CategoryController : Controller
{
    private readonly ApplicationDbContext _context;

    public CategoryController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var categories =  _context.Categories.ToList();
        return View(categories);
    }

    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(Category category)
    {
        if (ModelState.IsValid)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        return View(category);
    }
}
