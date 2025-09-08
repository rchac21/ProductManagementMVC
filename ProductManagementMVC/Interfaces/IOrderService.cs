using Microsoft.AspNetCore.Identity;
using ProductManagementMVC.Areas.Identity.Data;
using ProductManagementMVC.Entities;

using ProductManagementMVC.Models.OrderModels;


namespace ProductManagementMVC.Interfaces
{
    public interface IOrderService
    {
        IEnumerable<Order> GetOrders(string searchString);
        IEnumerable<Order> GetOrderByUserId(string searchString, string UserId);
        GetOrderResponse GetOrder(GetOrderRequest request);
        CreateOrderResponse CreateOrder(OrderModel request);
        UpdateOrderResponse UpdateOrder(UpdateOrderRequest request);
        DeleteOrderResponse DeleteOrder(DeleteOrderRequest request);
        List<ApplicationUser> GetAllUsers();
        IEnumerable<Product> GetProductsByCategoryName(string categoryName);
        IEnumerable<Product> GetChooseProduct(OrderModel model);
        IEnumerable<Product> GetProductList();
        Task<ApplicationUser> GetLoggedUser();
        //ApplicationUser GetUserById(string userId);
    }
}
