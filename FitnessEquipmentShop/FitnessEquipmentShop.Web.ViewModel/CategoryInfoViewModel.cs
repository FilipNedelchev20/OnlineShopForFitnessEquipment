using FitnessEquipmentShop.Data.Models.Entities;

namespace FitnessEquipmentShop.Web.ViewModel;
public class CategoryInfoViewModel
{
    public Category Category { get; set; }
    public IEnumerable<Product> Products { get; set; }
}
