
using Microsoft.AspNetCore.Mvc;
using ProductManagementMVC.Interfaces;
using ProductManagementMVC.Entities;
using ProductManagementMVC.Models;

namespace ProductManagementMVC.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public IActionResult AllCategories()
        {
            IEnumerable<Category> categories = _categoryService.GetCategories();
            return View(categories);
        }

        // GET
        public IActionResult Create()
        {
            return View();
        }

        // POST
        [HttpPost]
        public IActionResult Create(CategoryModel category)
        {
            CreateCategoryResponse createCategoryResponse = new CreateCategoryResponse();
            if (ModelState.IsValid)
            {
                createCategoryResponse = _categoryService.CreateCategory(category);
                return RedirectToAction("AllCategories");
            }
            return View(createCategoryResponse.CreatedCategory);
        }

        // GET
        public IActionResult Delete(int? id)
        {
            if(id == null || id == 0) {
                return NotFound();
            }
            var deleteCategoryResponse = _categoryService.GetCategory(new GetCategoryRequest() { Id = (int)id });
            if (deleteCategoryResponse == null)
            {
                return NotFound();
            }
            return View(deleteCategoryResponse.Category);
        }

        // POST
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            var getCategoryResponse =  _categoryService.GetCategory(new GetCategoryRequest() { Id = (int)id });
            if (getCategoryResponse == null) { 
                return NotFound();
            }
            _categoryService.DeleteCategory(new DeleteCategoryRequest() { Id = (int)id });
            return RedirectToAction("AllCategories");
        }

        // GET
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var deleteCategoryResponse = _categoryService.GetCategory(new GetCategoryRequest() { Id = (int)id });
            if (deleteCategoryResponse == null)
            {
                return NotFound();
            }
            return View(deleteCategoryResponse.Category);
        }

        // POST
        [HttpPost, ActionName("Edit")]
        public IActionResult Edit(CategoryModel category)
        {
            if (ModelState.IsValid)
            {
                _categoryService.UpdateCategory(new UpdateCategoryRequest() { CategoryToUpdate = category });
                return RedirectToAction("AllCategories");
            }
            return View(category);
        }
    }
}
