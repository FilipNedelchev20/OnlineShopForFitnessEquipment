using FitnessEquipmentShop.Data.Models.Entities;
using FitnessEquipmentShop.Web.ViewModel.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessEquipmentShop.Web.ViewModel.Home
{
    public class HomeViewModel
    {
        public IEnumerable<ProductViewModel> TopProducts { get; set; }

        public IEnumerable<CategoryViewModel> TopCategories { get; set; }
    }
}
