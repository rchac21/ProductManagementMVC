using System.ComponentModel.DataAnnotations;

namespace ProductManagementMVC.Entities
{
    public class Category
    {
        public int Id { get; set; }

        [MaxLength(10)]
        public string Name { get; set; }
        public string Code {  get; set; }
        public string Description { get; set; }
        // Navigation property (ერთ Category-ს ბევრი Product შეიძლება ქონდეს)
        public ICollection<Product> Products { get; set; }
    }
}
