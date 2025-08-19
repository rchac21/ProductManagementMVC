using ProductManagementMVC.Entity;
using ProductManagementMVC.Interfaces;
using ProductManagementMVC.Models;
using ProductManagementMVC.Entities;

namespace ProductManagementMVC.Mapping
{
    public class CategoryMapper : IMapper<Category, CategoryModel>
    {
        public CategoryModel MapFromEntityToModel(Category source) => new CategoryModel
        {
            Name = source.Name,
            Description = source.Description,
            Code = source.Code,
            Id = source.Id,
        };

        public Category MapFromModelToEntity(CategoryModel source)
        {
            var entity = new Category();

            MapFromModelToEntity(source, entity);

            return entity;
        }

        public void MapFromModelToEntity(CategoryModel source, Category target)
        {
            target.Name = source.Name;
            target.Description = source.Description;
            target.Code = source.Code;
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
