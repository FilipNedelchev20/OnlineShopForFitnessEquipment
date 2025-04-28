using Microsoft.EntityFrameworkCore.Metadata;

namespace FitnessEquipmentShop.Web.ViewModel
{
    public class ReviewViewModel
    {
        public ReviewViewModel() { }

        public int Id { get; set; }
        public string? Comment {  get; set; }
        public int Rating { get; set; }
        public int ProductId { get; set; }
    }
}
