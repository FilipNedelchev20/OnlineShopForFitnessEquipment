using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FitnessEquipmentShop.Data.Models.Entities
{
    public class Address
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        [Required]
        [StringLength(200)]
        public string Street { get; set; }

        [Required]
        [StringLength(100)]
        public string City { get; set; }


        [Required]
        [StringLength(10)]
        public string ZipCode { get; set; }

        [Required]
        [StringLength(50)]
        public string Country { get; set; }
    }
}