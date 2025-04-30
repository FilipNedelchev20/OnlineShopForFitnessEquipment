using System;
using System.Collections.Generic;
using FitnessEquipmentShop.Data.Models.Entities;

namespace FitnessEquipmentShop.Web.ViewModel
{
    public class OrderViewModel
    {
        public string UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public Address Address { get; set; }
        public List<OrderDetailViewModel> OrderDetails { get; set; }
    }
}
