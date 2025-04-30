using FitnessEquipmentShop.Services;
using FitnessEquipmentShop.Data.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[Authorize]
public class CategoryController : Controller
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        var categories = await _categoryService.GetAllCategoriesAsync();
        return View(categories);
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id)
    {
        var category = await _categoryService.GetCategoryByIdAsync(id);
        if (category == null)
        {
            return NotFound();
        }
        return View(category);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Category category)
    {
        if (id != category.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(category);
        }

        await _categoryService.UpdateCategoryAsync(category);
        TempData["SuccessMessage"] = "Category updated successfully!";

        // 👇 Redirect to Home Page
        return RedirectToAction("Index", "Home");
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var category = await _categoryService.GetCategoryByIdAsync(id);
        if (category == null)
        {
            return NotFound();
        }
        return View(category);
    }

    [HttpPost, ActionName("Delete")]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _categoryService.DeleteCategoryAsync(id);
        TempData["SuccessMessage"] = "Category deleted successfully!";

        // 👇 Redirect to Home Page after deletion too (optional)
        return RedirectToAction("Index", "Home");
    }
}
