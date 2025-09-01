using ProductManagementMVC.Entities;
using System.ComponentModel.DataAnnotations;

namespace ProductManagementMVC.Models.ProductModels
{
    public class CreateProductRequest
    {
        public int Id { get; set; }

        [MaxLength(10)]
        public string Name { get; set; }
        // Foreign Key
        [Required]
        public int CategoryId { get; set; }

        // Navigation property
        //public Category Category { get; set; }
        public double Price { get; set; }

    }
}
