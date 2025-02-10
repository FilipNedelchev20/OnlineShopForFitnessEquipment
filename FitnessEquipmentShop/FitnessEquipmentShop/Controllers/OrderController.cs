using FitnessEquipmentShop.Data.Models.Entities;
using FitnessEquipmentShop.Data;
using Microsoft.AspNetCore.Mvc;

public class OrderController : Controller
{
    private readonly ApplicationDbContext _context;

    public OrderController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var orders = _context.Orders.ToList();
        return View(orders);
    }

    public IActionResult Create()
    {
        var order = new Order { OrderDate = DateTime.Now };
        _context.Orders.Add(order);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }
}
