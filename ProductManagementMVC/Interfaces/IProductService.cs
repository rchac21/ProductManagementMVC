using ProductManagementMVC.Entities;

using ProductManagementMVC.Models.ProductModels;


namespace ProductManagementMVC.Interfaces
{
    public interface IProductService
    {
        IEnumerable<Product> GetProducts(string searchString);
        GetProductResponse GetProduct(GetProductRequest request);
        CreateProductResponse CreateProduct(ProductModel request);
        UpdateProductResponse UpdateProduct(UpdateProductRequest request);
        DeleteProductResponse DeleteProduct(DeleteProductRequest request);
    }
}
