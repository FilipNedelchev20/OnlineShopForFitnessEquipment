using FitnessEquipmentShop.Data.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessEquipmentShop.Web.ViewModel.Home
{
    public class HomeViewModel
    {
        public List<ProductViewModel> TopProducts { get; set; } = new List<ProductViewModel>();
    }
}
