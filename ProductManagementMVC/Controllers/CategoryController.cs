
using Microsoft.AspNetCore.Mvc;
using ProductManagementMVC.Interfaces;
using ProductManagementMVC.Entities;
using ProductManagementMVC.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace ProductManagementMVC.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        //public IActionResult AllCategories(string currentFilter, string searchString)
        //{
        //    IEnumerable<Category> categories = _categoryService.GetCategories(searchString);
        //    return View(categories);
        //}
        public IActionResult AllCategories(string currentFilter, string searchString, int pageNumber = 1, int pageSize = 5)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            // სერვისიდან მონაცემების წამოღება
            var categories = _categoryService.GetCategories(searchString);

            var totalCount = categories.Count();

            var items = categories
                .OrderBy(c => c.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            ViewBag.CurrentPage = pageNumber;

            return View(items);
        }

        // GET
        public IActionResult Create()
        {
            return View();
        }

        // POST
        [HttpPost]
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
            var updateCategoryResponse = _categoryService.GetCategory(new GetCategoryRequest() { Id = (int)id });
            if (updateCategoryResponse == null)
            {
                return NotFound();
            }
            return View(updateCategoryResponse.Category);
        }

        // POST
        [HttpPost, ActionName("Edit")]
        [Authorize(Roles = "Admin")]
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
