using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessEquipmentShop.Data.Models.Entities
{
    public class Product
    {
        public Product()
        {
            Reviews = new HashSet<Review>();
            OrderDetails = new HashSet<OrderDetail>();
        }
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Required]
        public int StockQuantity { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; }
        [Required]
        public int CategoryId { get; set; }

        [StringLength(500)]
        public string ImageUrl { get; set; }

        public double Rating { get; set; } // Average rating from reviews
        public ICollection<Review> Reviews { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }

    }
}
