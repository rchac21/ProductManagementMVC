using ProductManagementMVC.Entities;
using ProductManagementMVC.Models.CategoryModels;

namespace ProductManagementMVC.Interfaces
{
    public interface ICategoryService
    {
        IEnumerable<Category> GetCategories(string searchString);
        GetCategoryResponse GetCategory(GetCategoryRequest request);
        CreateCategoryResponse CreateCategory(CategoryModel request);
        UpdateCategoryResponse UpdateCategory(UpdateCategoryRequest request);
        DeleteCategoryResponse DeleteCategory(DeleteCategoryRequest request);
        List<Category> GetAllCategories();
    }
}
