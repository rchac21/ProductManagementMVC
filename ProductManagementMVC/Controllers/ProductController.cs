
using Microsoft.AspNetCore.Mvc;
using ProductManagementMVC.Interfaces;
using ProductManagementMVC.Entities;
using ProductManagementMVC.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using ProductManagementMVC.Models.ProductModels;
using ProductManagementMVC.Services;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProductManagementMVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ProductController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        //public IActionResult AllCategories(string currentFilter, string searchString)
        //{
        //    IEnumerable<Category> categories = _categoryService.GetCategories(searchString);
        //    return View(categories);
        //}
        public IActionResult AllProducts(string currentFilter, string searchString, int pageNumber = 1, int pageSize = 5)
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
            var products = _productService.GetProducts(searchString);

            var totalCount = products.Count();

            var items = products
                .OrderBy(c => c.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            ViewBag.CurrentPage = pageNumber;

            return View(items);
        }

        //// GET
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //// POST
        //[HttpPost]
        //[Authorize(Roles = "Admin")]
        //public IActionResult Create(ProductModel product)
        //{
        //    CreateProductResponse createProductResponse = new CreateProductResponse();
        //    if (ModelState.IsValid)
        //    {
        //        createProductResponse = _productService.CreateProduct(product);
        //        return RedirectToAction("AllProducts");
        //    }
        //    return View(createProductResponse.CreatedProduct);
        //}

        // GET: Product/Create
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_categoryService.GetAllCategories(), "Id", "Name");
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(ProductModel product)
        {
            if (ModelState.IsValid)
            {
                var response = _productService.CreateProduct(product);
                return RedirectToAction("AllProducts");
            }

            // თუ ვალიდაცია ჩავარდა, კატეგორიები ისევ დაბრუნდეს dropdown-ში
            ViewBag.Categories = new SelectList(_categoryService.GetAllCategories(), "Id", "Name", product.CategoryId);
            return View(product);
        }

        // GET
        public IActionResult Delete(int? id)
        {
            if(id == null || id == 0) {
                return NotFound();
            }
            var deleteProductResponse = _productService.GetProduct(new GetProductRequest() { Id = (int)id });
            if (deleteProductResponse == null)
            {
                return NotFound();
            }
            return View(deleteProductResponse.Product);
        }

        // POST
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeletePOST(int? id)
        {
            var getProductResponse = _productService.GetProduct(new GetProductRequest() { Id = (int)id });
            if (getProductResponse == null) { 
                return NotFound();
            }
            _productService.DeleteProduct(new DeleteProductRequest() { Id = (int)id });
            return RedirectToAction("AllProducts");
        }

        // GET
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var updateProductResponse = _productService.GetProduct(new GetProductRequest() { Id = (int)id });
            if (updateProductResponse == null)
            {
                return NotFound();
            }
            return View(updateProductResponse.Product);
        }

        // POST
        [HttpPost, ActionName("Edit")]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(ProductModel product)
        {
            if (ModelState.IsValid)
            {
                _productService.UpdateProduct(new UpdateProductRequest() { ProductToUpdate = product });
                return RedirectToAction("AllProducts");
            }
            return View(product);
        }
    }
}
