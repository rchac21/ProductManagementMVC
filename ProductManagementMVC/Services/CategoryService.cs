using Microsoft.AspNetCore.Mvc.Infrastructure;
using ProductManagementMVC.Interfaces;
using Microsoft.EntityFrameworkCore;
using ProductManagementMVC.Mapping;
using ProductManagementMVC.Models;
using ProductManagementMVC.Data;
using ProductManagementMVC.Entities;

namespace ProductManagementMVC.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ProductManagementMVCContext _context;
        private readonly IMapper<Entities.Category, CategoryModel> _categoryMapper;

        public CategoryService(ProductManagementMVCContext context)
        {
            _categoryMapper = new CategoryMapper();
            _context = context;
        }

         

        public CreateCategoryResponse CreateCategory(CategoryModel category)
        {
            var categoryAlreadyExists = _context.Categories.Any(p => p.Id == category.Id);

            if (categoryAlreadyExists)
            {
                throw new DbUpdateException($"Category with id, '{category.Id}' already exist.");
            }

            var newCategory = _context.Categories.Add(_categoryMapper.MapFromModelToEntity(category));

            _context.SaveChanges();

            return new CreateCategoryResponse { CreatedCategory = _categoryMapper.MapFromEntityToModel(newCategory.Entity) };
        }

        public GetCategoryResponse GetCategory(GetCategoryRequest getCategoryRequest)
        {
            var category = _context.Categories.Find(getCategoryRequest.Id);

            return new GetCategoryResponse { Category = _categoryMapper.MapFromEntityToModel(category) };
        }

        public UpdateCategoryResponse UpdateCategory(UpdateCategoryRequest updateCategoryRequest)
        {
            var existingCategoryToUpdate = _context.Categories.Find(updateCategoryRequest.CategoryToUpdate.Id);

            if (existingCategoryToUpdate == null)
            {
                throw new DbUpdateException($"Category whith Id {updateCategoryRequest.CategoryToUpdate.Id} does not exist ");
            }

            _categoryMapper.MapFromModelToEntity(updateCategoryRequest.CategoryToUpdate, existingCategoryToUpdate);

            _context.SaveChanges();

            return new UpdateCategoryResponse { UpdateCategory = updateCategoryRequest.CategoryToUpdate };
        }

        public DeleteCategoryResponse DeleteCategory(DeleteCategoryRequest deleteCategoryRequest)
        {
            var existingCategoryToDelete = _context.Categories.Find(deleteCategoryRequest.Id);

            if (existingCategoryToDelete == null)
            {
                throw new DbUpdateException($"Category whith Id {deleteCategoryRequest.Id} does not exist ");
            }

            _context.Categories.Remove(existingCategoryToDelete);
            _context.SaveChanges();

            return new DeleteCategoryResponse();
        }
        public IEnumerable<Category> GetCategories(string searchString)
        {
            return _context.Categories.Where(x => string.IsNullOrEmpty(searchString) || x.Name.Contains(searchString)); 
        }
    }
}
