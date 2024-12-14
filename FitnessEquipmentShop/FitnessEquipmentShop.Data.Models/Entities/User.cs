using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessEquipmentShop.Data.Models.Entities
{
    public class User
    {
        public User()
        {
            Addresses = new HashSet<Address>();
            Wishlists = new HashSet<Wishlist>();
            Orders = new HashSet<Order>();
            Reviews = new HashSet<Review>();
        }
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(20)]
        public string Role { get; set; } 

        [Required]
        public DateTime RegistrationDate { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<Address> Addresses { get; set; }
        public ICollection<Wishlist> Wishlists { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
