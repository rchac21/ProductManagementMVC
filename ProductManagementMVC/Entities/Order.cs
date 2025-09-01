using Microsoft.AspNetCore.Identity;
using ProductManagementMVC.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductManagementMVC.Entities
{
    public class Order
    {
        public int Id { get; set; }
        // FK to AspNetUsers
        public string UserId { get; set; }
        // Navigation property
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
        public int? Drink { get; set; }
        public int? Food { get; set; }
        public int? Sweet { get; set; }
        // Navigation properties Products-ზე
        [ForeignKey("Drink")]
        public virtual Product DrinkNavigation { get; set; }

        [ForeignKey("Food")]
        public virtual Product FoodNavigation { get; set; }

        [ForeignKey("Sweet")]
        public virtual Product SweetNavigation { get; set; }
        public int? Amount { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
