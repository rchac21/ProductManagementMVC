using ProductManagementMVC.Interfaces;
using ProductManagementMVC.Models.OrderModels;
using ProductManagementMVC.Entities;


namespace ProductManagementMVC.Mapping
{
    public class OrderMapper : IMapper<Order, OrderModel>
    {
        
        public OrderModel MapFromEntityToModel(Order source) => new OrderModel
        {
            Id = source.Id,
            UserId = source.UserId,
            Drink = source.Drink,
            Food = source.Food,
            Sweet = source.Sweet,
            Amount = source.Amount,
            User = source.User,
            CreatedDate = source.CreatedDate,

            // UI-სთვის სახელები
            DrinkName = source.DrinkNavigation?.Name,
            FoodName = source.FoodNavigation?.Name,
            SweetName = source.SweetNavigation?.Name
        };

        public Order MapFromModelToEntity(OrderModel source)
        {
            var entity = new Order();

            MapFromModelToEntity(source, entity);

            return entity;

        }

        public void MapFromModelToEntity(OrderModel source, Order target)
        {
            target.Id = source.Id;
            target.UserId = source.UserId;
            target.Drink = source.Drink;
            target.Food = source.Food;
            target.Sweet = source.Sweet;
            target.Amount = source.Amount;
            target.User = source.User;
            target.CreatedDate = DateTime.Now;
        }

       
    }
}
