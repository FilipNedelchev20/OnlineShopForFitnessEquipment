using FitnessEquipmentShop.Data.Models.Entities;
using FitnessEquipmentShop.Services;
using FitnessEquipmentShop.Web.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
    public async Task<IActionResult> Index(string searchTerm, int? selectedCategoryId, decimal? minPrice, decimal? maxPrice, string sortBy, int page = 1)
    {
        const int pageSize = 6;

        var products = await _productService.GetAllProductsAsync();
        var categories = await _categoryService.GetAllCategoriesAsync();

        // Ensure defaults if missing (important for slider and logic)
        if (!minPrice.HasValue) minPrice = 0;
        if (!maxPrice.HasValue) maxPrice = 5000;

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            products = products.Where(p => p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        if (selectedCategoryId.HasValue)
        {
            products = products.Where(p => p.CategoryId == selectedCategoryId.Value).ToList();
        }

        if (minPrice.HasValue)
        {
            products = products.Where(p => p.Price >= minPrice.Value).ToList();
        }

        if (maxPrice.HasValue)
        {
            products = products.Where(p => p.Price <= maxPrice.Value).ToList();
        }

        if (!string.IsNullOrEmpty(sortBy))
        {
            products = sortBy switch
            {
                "rating" => products.OrderByDescending(p => p.Reviews.Any() ? p.Reviews.Average(r => r.Rating) : 0).ToList(),
                "price-asc" => products.OrderBy(p => p.Price).ToList(),
                "price-desc" => products.OrderByDescending(p => p.Price).ToList(),
                _ => products
            };
        }

        var totalProducts = products.Count();
        var totalPages = (int)Math.Ceiling(totalProducts / (double)pageSize);
        var pagedProducts = products.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        var viewModel = new ProductFilterViewModel
        {
            Products = pagedProducts,
            Categories = categories,
            SearchTerm = searchTerm,
            SelectedCategoryId = selectedCategoryId,
            MinPrice = minPrice,
            MaxPrice = maxPrice,
            SortBy = sortBy,
            Page = page,
            TotalPages = totalPages
        };

        return View(viewModel);
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
    [HttpGet]
    public async Task<IActionResult> SearchSuggestions(string term)
    {
        var allProducts = await _productService.GetAllProductsAsync();

        var suggestions = allProducts
            .Where(p => p.Name.Contains(term, StringComparison.OrdinalIgnoreCase))
            .Select(p => new { id = p.Id, name = p.Name })
            .Take(5)
            .ToList();

        return Json(suggestions);
    }
    public async Task<IActionResult> CategoryInfo(int categoryId)
    {
        var category = await _categoryService.GetCategoryByIdAsync(categoryId);
        var products = await _productService.GetProductsByCategoryIdAsync(categoryId);

        var viewModel = new CategoryInfoViewModel
        {
            Category = category,
            Products = products
        };

        return View(viewModel);
    }


}
