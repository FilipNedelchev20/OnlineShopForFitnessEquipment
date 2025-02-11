using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessEquipmentShop.Data.Models.Entities
{
    public class Review
    {
        [Key]
        public int Id { get; set; }


        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }
        [Required]
        public int ProductId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
        [Required]
        public string UserId { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; } 

        [StringLength(1000)]
        public string Comment { get; set; }

        [Required]
        public DateTime DatePosted { get; set; }
    }
}
