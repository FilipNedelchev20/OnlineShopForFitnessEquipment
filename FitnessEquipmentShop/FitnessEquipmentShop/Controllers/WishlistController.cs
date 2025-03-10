using FitnessEquipmentShop.Data.Models.Entities;
using FitnessEquipmentShop.Data;
using Microsoft.AspNetCore.Mvc;

public class WishlistController : Controller
{
    private readonly ApplicationDbContext _context;

    public WishlistController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var wishlist = _context.Wishlists.ToList();
        return View(wishlist);
    }

    public async Task<IActionResult> Add(int productId)
    {
        var product = await _context.Products.FindAsync(productId);
        if (product == null) return NotFound();

        var wishlistItem = new Wishlist { ProductId = productId };
        await _context.Wishlists.AddAsync(wishlistItem);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }
}
