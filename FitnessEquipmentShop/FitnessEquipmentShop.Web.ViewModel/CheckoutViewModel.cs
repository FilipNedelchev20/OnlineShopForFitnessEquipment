using FitnessEquipmentShop.Data.Models.Entities;

namespace FitnessEquipmentShop.Web.ViewModel
{
    public class CheckoutViewModel
    {
        public List<CartItem> CartItems { get; set; }
        public decimal TotalPrice { get; set; }
        public Address Address { get; set; } 
    }

}
