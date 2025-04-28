using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FitnessEquipmentShop.Data.Models.Entities
{
    public class Review
    {
        [Key]
        public int Id { get; set; }


        [JsonIgnore]
        public Product Product { get; set; }
        [Required]
        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }

        [JsonIgnore]
        public User User { get; set; }
        [Required]
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; } 

        [StringLength(1000)]
        public string Comment { get; set; }

        //[Required]
        //public DateTime DatePosted { get; set; }
    }
}
