using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessEquipmentShop.Data.Models.Entities
{
    public class OrderDetail
    {
        [Key]
        public int Id { get; set; }


        [ForeignKey(nameof(OrderId))]
        public Order Order { get; set; }
        [Required]
        public int OrderId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }
        [Required]
        public int ProductId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }
    }
}
