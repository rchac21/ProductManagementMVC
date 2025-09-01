using ProductManagementMVC.Entities;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using ProductManagementMVC.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProductManagementMVC.Models.OrderModels
{
    public class OrderModel
    {
        public int Id { get; set; }

        // FK to AspNetUsers
        [Required(ErrorMessage = "Please select a user.")]
        public string UserId { get; set; }

        // Navigation property
        [ValidateNever]
        public virtual ApplicationUser User { get; set; }

        public int? Drink { get; set; }
        public int? Food { get; set; }
        public int? Sweet { get; set; }
        public int? Amount { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        // გამოტანითი ველები (UI)
        public string? DrinkName { get; set; }
        public string? FoodName { get; set; }
        public string? SweetName { get; set; }

        // Dropdown სიისთვის (enum-ის მაგივრად)
        //public IEnumerable<SelectListItem>? Drinks { get; set; }
        //public IEnumerable<SelectListItem>? Foods { get; set; }
        //public IEnumerable<SelectListItem>? Sweets { get; set; }
        // Computed property
        public string? UserName => User?.UserName;
    }

}
