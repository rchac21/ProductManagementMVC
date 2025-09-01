using Microsoft.AspNetCore.Mvc.Infrastructure;
using ProductManagementMVC.Interfaces;
using Microsoft.EntityFrameworkCore;
using ProductManagementMVC.Mapping;
using ProductManagementMVC.Data;
using ProductManagementMVC.Entities;
using ProductManagementMVC.Models.ProductModels;

namespace ProductManagementMVC.Services
{
    public class ProductService : IProductService
    {
        private readonly ProductManagementMVCContext _context;
        private readonly IMapper<Product, ProductModel> _productMapper;

        public ProductService(ProductManagementMVCContext context)
        {
            _productMapper = new ProductMapper();
            _context = context;
        }

         

        public CreateProductResponse CreateProduct(ProductModel product)
        {
            var productAlreadyExists = _context.Products.Any(p => p.Id == product.Id);

            if (productAlreadyExists)
            {
                throw new DbUpdateException($"Product with id, '{product.Id}' already exist.");
            }

            var newProduct = _context.Products.Add(_productMapper.MapFromModelToEntity(product));

            _context.SaveChanges();

            return new CreateProductResponse { CreatedProduct = _productMapper.MapFromEntityToModel(newProduct.Entity) };
        }

        public GetProductResponse GetProduct(GetProductRequest getProductRequest)
        {
            var product = _context.Products.Find(getProductRequest.Id);

            return new GetProductResponse { Product = _productMapper.MapFromEntityToModel(product) };
        }

        public UpdateProductResponse UpdateProduct(UpdateProductRequest updateProductRequest)
        {
            var existingProductToUpdate = _context.Products.Find(updateProductRequest.ProductToUpdate.Id);

            if (existingProductToUpdate == null)
            {
                throw new DbUpdateException($"Product whith Id {updateProductRequest.ProductToUpdate.Id} does not exist ");
            }

            _productMapper.MapFromModelToEntity(updateProductRequest.ProductToUpdate, existingProductToUpdate);

            _context.SaveChanges();

            return new UpdateProductResponse { UpdateProduct = updateProductRequest.ProductToUpdate };
        }

        public DeleteProductResponse DeleteProduct(DeleteProductRequest deleteProductRequest)
        {
            var existingProductToDelete = _context.Products.Find(deleteProductRequest.Id);

            if (existingProductToDelete == null)
            {
                throw new DbUpdateException($"Product whith Id {deleteProductRequest.Id} does not exist ");
            }

            _context.Products.Remove(existingProductToDelete);
            _context.SaveChanges();

            return new DeleteProductResponse();
        }
        public IEnumerable<Product> GetProducts(string searchString)
        {
            return _context.Products.Where(x => string.IsNullOrEmpty(searchString) || x.Name.Contains(searchString)); 
        }
    }
}
