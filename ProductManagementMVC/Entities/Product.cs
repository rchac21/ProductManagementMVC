using System.ComponentModel.DataAnnotations;

namespace ProductManagementMVC.Entities
{
    public class Product
    {
        public int Id { get; set; }

        [MaxLength(10)]
        public string Name { get; set; }
        // Foreign Key
        public int CategoryId { get; set; }

        // Navigation property
        public Category Category { get; set; }
        public double Price { get; set; }
    }
}
