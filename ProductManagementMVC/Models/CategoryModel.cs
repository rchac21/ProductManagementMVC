using System.ComponentModel.DataAnnotations;

namespace ProductManagementMVC.Models
{
    public class CategoryModel
    {
        public int Id { get; set; }
        [MaxLength(10)]
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }
}
