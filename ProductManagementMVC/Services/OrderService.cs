using Microsoft.AspNetCore.Mvc.Infrastructure;
using ProductManagementMVC.Interfaces;
using Microsoft.EntityFrameworkCore;
using ProductManagementMVC.Mapping;
using ProductManagementMVC.Data;
using ProductManagementMVC.Entities;
using ProductManagementMVC.Models.OrderModels;
using Microsoft.AspNetCore.Identity;
using ProductManagementMVC.Areas.Identity.Data;

namespace ProductManagementMVC.Services
{
    public class OrderService : IOrderService
    {
        private readonly ProductManagementMVCContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper<Order, OrderModel> _orderMapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderService(ProductManagementMVCContext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _orderMapper = new OrderMapper();
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }



        public CreateOrderResponse CreateOrder(OrderModel order)
        {
            var orderAlreadyExists = _context.Orders.Any(p => p.Id == order.Id);

            if (orderAlreadyExists)
            {
                throw new DbUpdateException($"Order with id, '{order.Id}' already exist.");
            }

            var newOrder = _context.Orders.Add(_orderMapper.MapFromModelToEntity(order));

            _context.SaveChanges();

            return new CreateOrderResponse { CreatedOrder = _orderMapper.MapFromEntityToModel(newOrder.Entity) };
        }

        public GetOrderResponse GetOrder(GetOrderRequest getOrderRequest)
        {
            //var orders = _context.Orders
            //    .Include(o => o.DrinkNavigation)
            //    .Include(o => o.FoodNavigation)
            //    .Include(o => o.SweetNavigation)
            //    .Include(o => o.User)
            //.ToList();
            //var order = _context.Orders.Find(getOrderRequest.Id);
            var order = _context.Orders
                .Include(o => o.DrinkNavigation)
                .Include(o => o.FoodNavigation)
                .Include(o => o.SweetNavigation)
                .Include(o => o.User)
                .FirstOrDefault(o => o.Id == getOrderRequest.Id);


            return new GetOrderResponse { Order = _orderMapper.MapFromEntityToModel(order) };
        }

        public UpdateOrderResponse UpdateOrder(UpdateOrderRequest updateOrderRequest)
        {
            var existingOrderToUpdate = _context.Orders.Find(updateOrderRequest.OrderToUpdate.Id);

            if (existingOrderToUpdate == null)
            {
                throw new DbUpdateException($"Order whith Id {updateOrderRequest.OrderToUpdate.Id} does not exist ");
            }

            _orderMapper.MapFromModelToEntity(updateOrderRequest.OrderToUpdate, existingOrderToUpdate);

            _context.SaveChanges();

            return new UpdateOrderResponse { UpdateOrder = updateOrderRequest.OrderToUpdate };
        }

        public DeleteOrderResponse DeleteOrder(DeleteOrderRequest deleteOrderRequest)
        {
            var existingOrderToDelete = _context.Orders.Find(deleteOrderRequest.Id);

            if (existingOrderToDelete == null)
            {
                throw new DbUpdateException($"Order whith Id {deleteOrderRequest.Id} does not exist ");
            }

            _context.Orders.Remove(existingOrderToDelete);
            _context.SaveChanges();

            return new DeleteOrderResponse();
        }
        //public IEnumerable<Order> GetOrders(string searchString)
        //{
        //    var orders = _context.Orders
        //        .Include(o => o.DrinkNavigation)
        //        .Include(o => o.FoodNavigation)
        //        .Include(o => o.SweetNavigation)
        //        .Include(o => o.User)
        //    .ToList();

        //    return orders.Where(x => string.IsNullOrEmpty(searchString) || x.UserId.Contains(searchString));
        //}

        public IEnumerable<Order> GetOrders(string searchString)
        {
            var orders = _context.Orders
                .Include(o => o.DrinkNavigation)

                .Include(o => o.FoodNavigation)
                .Include(o => o.SweetNavigation)
                .Include(o => o.User)
                .ToList();

            if (!string.IsNullOrEmpty(searchString))
            {
                orders = orders.Where(o => o.User.UserName.Contains(searchString)).ToList();
            }

            return orders;
        }


        public IEnumerable<Order> GetOrderByUserId(string searchString, string userId)
        {
            var orders = _context.Orders
                .Include(o => o.DrinkNavigation)
                .Include(o => o.FoodNavigation)
                .Include(o => o.SweetNavigation)
                .Include(o => o.User)
                .Where(o => o.UserId == userId);

            if (!string.IsNullOrEmpty(searchString))
            {
                orders = orders.Where(o => o.User.UserName.Contains(searchString));
            }

            return orders.ToList();
        }


        //public IEnumerable<Order> GetOrderByUserId(string searchString, string userId)
        //{
        //    var orders = _context.Orders
        //        .Include(o => o.DrinkNavigation)
        //        .Include(o => o.FoodNavigation)
        //        .Include(o => o.SweetNavigation)
        //        .Include(o => o.User)
        //        .Where(o => o.UserId == userId)   // ✅ მხოლოდ ამ იუზერის ჩანაწერები
        //        .ToList();

        //    return orders.Where(x => string.IsNullOrEmpty(searchString)
        //                             || x.User.UserName.Contains(searchString));
        //}

        public List<ApplicationUser> GetAllUsers()
        {
            return _userManager.Users.ToList();
        }

        public IEnumerable<Product> GetProductsByCategoryName(string categoryName)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Name == categoryName);
            if (category == null)
                return new List<Product>();

            return _context.Products.Where(p => p.CategoryId == category.Id).ToList();
        }

        //public IEnumerable<Product> GetProductList()
        //{
        //    return _context.Products.ToList();
        //}
        public IEnumerable<Product> GetProductList()
        {
            return _context.Products
                .Include(p => p.Category) // Category ჩატვირთავს
                .ToList();
        }


        public async Task<ApplicationUser> GetLoggedUser()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null)
                return null;

            return await _userManager.GetUserAsync(user);
        }

        public IEnumerable<Product> GetChooseProduct(OrderModel model)
        {
            var selectedProducts = _context.Products
            .Where(p => p.Id == model.Drink || p.Id == model.Food || p.Id == model.Sweet)
            .ToList();

            return selectedProducts;
        }


        //public ApplicationUser GetUserById(string userId)
        //{
        //    // _userManager არის UserManager<ApplicationUser>
        //    return _userManager.Users.FirstOrDefault(u => u.Id == userId);
        //}

    }
}
