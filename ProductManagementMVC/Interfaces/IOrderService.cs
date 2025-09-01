using Microsoft.AspNetCore.Identity;
using ProductManagementMVC.Areas.Identity.Data;
using ProductManagementMVC.Entities;

using ProductManagementMVC.Models.OrderModels;


namespace ProductManagementMVC.Interfaces
{
    public interface IOrderService
    {
        IEnumerable<Order> GetOrders(string searchString);
        GetOrderResponse GetOrder(GetOrderRequest request);
        CreateOrderResponse CreateOrder(OrderModel request);
        UpdateOrderResponse UpdateOrder(UpdateOrderRequest request);
        DeleteOrderResponse DeleteOrder(DeleteOrderRequest request);
        List<ApplicationUser> GetAllUsers();
        IEnumerable<Product> GetProductsByCategoryName(string categoryName);
        //ApplicationUser GetUserById(string userId);
    }
}
