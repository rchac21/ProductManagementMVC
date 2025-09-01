
using Microsoft.AspNetCore.Mvc;using ProductManagementMVC.Interfaces;
using ProductManagementMVC.Entities;
using ProductManagementMVC.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using ProductManagementMVC.Models.OrderModels;
using ProductManagementMVC.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProductManagementMVC.Mapping;

namespace ProductManagementMVC.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IMapper<Order, OrderModel> _orderMapper;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
            _orderMapper = new OrderMapper();
        }

        //public IActionResult AllOrders(string currentFilter, string searchString, int pageNumber = 1, int pageSize = 5)
        //{
        //    if (!string.IsNullOrEmpty(searchString))
        //    {
        //        pageNumber = 1;
        //    }
        //    else
        //    {
        //        searchString = currentFilter;
        //    }

        //    ViewData["CurrentFilter"] = searchString;

        //    // სერვისიდან მონაცემების წამოღება
        //    var orders = _orderService.GetOrders(searchString);

        //    var totalCount = orders.Count();

        //    var items = orders
        //        .OrderBy(c => c.Id)
        //        .Skip((pageNumber - 1) * pageSize)
        //        .Take(pageSize)
        //        .ToList();

        //    ViewBag.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        //    ViewBag.CurrentPage = pageNumber;

        //    return View(items);
        //}

        public IActionResult AllOrders(string currentFilter, string searchString, int pageNumber = 1, int pageSize = 5)
        {
            if (!string.IsNullOrEmpty(searchString))
                pageNumber = 1;
            else
                searchString = currentFilter;

            ViewData["CurrentFilter"] = searchString;

            var orders = _orderService.GetOrders(searchString);

            var orderModels = orders.Select(o => _orderMapper.MapFromEntityToModel(o)).ToList();

            var totalCount = orderModels.Count();
            var items = orderModels
                .OrderBy(c => c.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            ViewBag.CurrentPage = pageNumber;

            return View(items);
        }



        public IActionResult Create()
        {
            ViewBag.Users = new SelectList(_orderService.GetAllUsers(), "Id", "UserName");
            // Products category მიხედვით
            ViewBag.Drinks = new SelectList(_orderService.GetProductsByCategoryName("Drink"), "Id", "Name");
            ViewBag.Foods = new SelectList(_orderService.GetProductsByCategoryName("Food"), "Id", "Name");
            ViewBag.Sweets = new SelectList(_orderService.GetProductsByCategoryName("Sweet"), "Id", "Name");

            return View();
        }

        // POST: Order/Create
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(OrderModel order)
        {
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine(error.ErrorMessage);
            }

            if (ModelState.IsValid)
            {
                var response = _orderService.CreateOrder(order);
                return RedirectToAction("AllOrders");
            }

            // თუ ვალიდაცია ჩავარდა, კატეგორიები ისევ დაბრუნდეს dropdown-ში
            ViewBag.Users = new SelectList(_orderService.GetAllUsers(), "Id", "UserName", order.UserId);
            ViewBag.Drinks = new SelectList(_orderService.GetProductsByCategoryName("Drink"), "Id", "Name", order.Drink);
            ViewBag.Foods = new SelectList(_orderService.GetProductsByCategoryName("Food"), "Id", "Name", order.Food);
            ViewBag.Sweets = new SelectList(_orderService.GetProductsByCategoryName("Sweet"), "Id", "Name", order.Sweet);

            return View(order);
        }

        // GET
        public IActionResult Delete(int? id)
        {
            if(id == null || id == 0) {
                return NotFound();
            }
            var deleteOrderResponse = _orderService.GetOrder(new GetOrderRequest() { Id = (int)id });
            if (deleteOrderResponse == null)
            {
                return NotFound();
            }
            return View(deleteOrderResponse.Order);
        }

        // POST
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeletePOST(int? id)
        {
            var getOrderResponse = _orderService.GetOrder(new GetOrderRequest() { Id = (int)id });
            if (getOrderResponse == null) { 
                return NotFound();
            }
            _orderService.DeleteOrder(new DeleteOrderRequest() { Id = (int)id });
            return RedirectToAction("AllOrders");
        }

        // GET
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var updateOrderResponse = _orderService.GetOrder(new GetOrderRequest() { Id = (int)id });
            if (updateOrderResponse == null)
            {
                return NotFound();
            }
            // Products category მიხედვით
            ViewBag.Drinks = new SelectList(_orderService.GetProductsByCategoryName("Drink"), "Id", "Name");
            ViewBag.Foods = new SelectList(_orderService.GetProductsByCategoryName("Food"), "Id", "Name");
            ViewBag.Sweets = new SelectList(_orderService.GetProductsByCategoryName("Sweet"), "Id", "Name");
            return View(updateOrderResponse.Order);
        }

        // POST
        [HttpPost, ActionName("Edit")]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(OrderModel order)
        {
            if (ModelState.IsValid)
            {
                _orderService.UpdateOrder(new UpdateOrderRequest() { OrderToUpdate = order });
                return RedirectToAction("AllOrders");
            }
            // Products category მიხედვით
            ViewBag.Drinks = new SelectList(_orderService.GetProductsByCategoryName("Drink"), "Id", "Name");
            ViewBag.Foods = new SelectList(_orderService.GetProductsByCategoryName("Food"), "Id", "Name");
            ViewBag.Sweets = new SelectList(_orderService.GetProductsByCategoryName("Sweet"), "Id", "Name");
            return View(order);
        }
    }
}
