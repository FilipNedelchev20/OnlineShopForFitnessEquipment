
using Microsoft.AspNetCore.Identity;

namespace FitnessEquipmentShop.Data.Models.Entities
{
    public class User : IdentityUser
    {
        public string? FullName { get; set; } = null!;
        public string? ProfilePicturePath { get; set; } = null!;
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
    }

}
