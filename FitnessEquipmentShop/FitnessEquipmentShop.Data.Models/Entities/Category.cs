using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessEquipmentShop.Data.Models.Entities
{
    public class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
