using FitnessEquipmentShop.Data.Models.Entities;
namespace FitnessEquipmentShop.Web.ViewModel;
public class ProductFilterViewModel
{
    public IEnumerable<Product> Products { get; set; }
    public IEnumerable<Category> Categories { get; set; }

    public string SearchTerm { get; set; }
    public int? SelectedCategoryId { get; set; }
    public string SortBy { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }

    public int Page { get; set; }
    public int TotalPages { get; set; }
}
