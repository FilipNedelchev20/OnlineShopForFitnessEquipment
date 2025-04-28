using FitnessEquipmentShop.Data.Models.Entities;
using FitnessEquipmentShop.Services;
using FitnessEquipmentShop.Web.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

public class ProductController : Controller
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;

    public ProductController(IProductService productService, ICategoryService categoryService)
    {
        _productService = productService;
        _categoryService = categoryService;
    }

    // Accessible by everyone
    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        var products = await _productService.GetAllProductsAsync();
        return View(products);
    }

    // Accessible by everyone
    [AllowAnonymous]
    public async Task<IActionResult> Details(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        return View(product);
    }

    // Only Admin can create products
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create()
    {
        ViewBag.Categories = new SelectList(await _categoryService.GetAllCategoriesAsync(), "Id", "Name");
        return View();
    }

    // Only Admin can create products
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateProductViewModel productVm)
    {
        if (ModelState.IsValid)
        {
            var product = new Product
            {
                Name = productVm.Name,
                Description = productVm.Description,
                Price = productVm.Price,
                CategoryId = productVm.CategoryId,
                StockQuantity = productVm.StockQuantity,
                ImageUrl = productVm.ImageUrl,
                // Store the admin's username (or ID if you prefer)
                CreatedBy = User.Identity.Name
            };

            await _productService.CreateProductAsync(product);
            return RedirectToAction("Index");
        }
        ViewBag.Categories = new SelectList(await _categoryService.GetAllCategoriesAsync(), "Id", "Name", productVm.CategoryId);
        return View(productVm);
    }
    public async Task<IActionResult> ByCategory(int categoryId)
    {
        var products = await _productService.GetAllProductsAsync();
        var filteredProducts = products
            .Where(p => p.CategoryId == categoryId)
            .ToList();

        return View("Index", filteredProducts); // Използваме същия view за продукти
    }

    // Only Admin can edit products
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        var viewModel = new EditProductViewModel
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            StockQuantity = product.StockQuantity,
            CategoryId = product.CategoryId,
            ImageUrl = product.ImageUrl
        };

        ViewBag.Categories = new SelectList(await _categoryService.GetAllCategoriesAsync(), "Id", "Name", product.CategoryId);
        return View(viewModel);
    }

    // Only Admin can edit products
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, EditProductViewModel model)
    {
        if (id != model.Id)
        {
            return BadRequest();
        }

        if (ModelState.IsValid)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            product.Name = model.Name;
            product.Description = model.Description;
            product.Price = model.Price;
            product.StockQuantity = model.StockQuantity;
            product.CategoryId = model.CategoryId;
            product.ImageUrl = model.ImageUrl;

            await _productService.UpdateProductAsync(product);
            return RedirectToAction("Index");
        }

        ViewBag.Categories = new SelectList(await _categoryService.GetAllCategoriesAsync(), "Id", "Name", model.CategoryId);
        return View(model);
    }

    // Only Admin can delete products
    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _productService.DeleteProductAsync(id);
        TempData["SuccessMessage"] = "Product deleted successfully!";
        return RedirectToAction("Index");
    }
}
