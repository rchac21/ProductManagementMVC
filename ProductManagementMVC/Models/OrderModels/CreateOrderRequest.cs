using Microsoft.AspNetCore.Identity;
using ProductManagementMVC.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductManagementMVC.Models.OrderModels
{
    public class CreateOrderRequest
    {
        public int Id { get; set; }
        // FK to AspNetUsers
        public string UserId { get; set; }
        //// Navigation property
        //[ForeignKey("UserId")]
        //public virtual IdentityUser User { get; set; }
        public string? Drink { get; set; }
        public string? Food { get; set; }
        public string? Sweet { get; set; }
        public int? Amount { get; set; }
        //public DateTime CreatedDate { get; set; } = DateTime.Now;

    }
}
