
using Microsoft.AspNetCore.Identity;

namespace FitnessEquipmentShop.Data.Models.Entities
{
    public class User : IdentityUser 
    {
        public string FullName { get; set; } 
    }
}
