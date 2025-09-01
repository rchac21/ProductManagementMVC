using ProductManagementMVC.Interfaces;
using ProductManagementMVC.Models.ProductModels;
using ProductManagementMVC.Entities;

namespace ProductManagementMVC.Mapping
{
    public class ProductMapper : IMapper<Product, ProductModel>
    {
        public ProductModel MapFromEntityToModel(Product source) => new ProductModel
        {
            Id = source.Id,
            Name = source.Name,
            Price = source.Price,
            CategoryId = source.CategoryId,
        };

        public Product MapFromModelToEntity(ProductModel source)
        {
            var entity = new Product();

            MapFromModelToEntity(source, entity);

            return entity;
        }

        public void MapFromModelToEntity(ProductModel source, Product target)
        {
            target.Name = source.Name;
            target.Price = source.Price;
            target.CategoryId = source.CategoryId;
            target.Id = source.Id;
        }

        //public Entity.Category MapFromModelToEntity(CategoryModel source)
        //{
        //    var entity = new Entity.Category();

        //    MapFromModelToEntity(source, entity);

        //    return entity;
        //}

        //public void MapFromModelToEntity(CategoryModel source, Entity.Category target)
        //{
        //    target.Name = source.Name;
        //    target.Description = source.Description;
        //    target.Code = source.Code;
        //    target.Id = source.Id;
        //}
    }
}
