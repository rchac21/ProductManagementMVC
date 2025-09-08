
using Microsoft.AspNetCore.Mvc;using ProductManagementMVC.Interfaces;
using ProductManagementMVC.Entities;
using ProductManagementMVC.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using ProductManagementMVC.Models.OrderModels;
using ProductManagementMVC.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProductManagementMVC.Mapping;
using Microsoft.AspNetCore.Identity;
using SendGrid.Helpers.Mail;
using EllipticCurve.Utils;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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

        public IActionResult OrderSuccess()
        {
            ViewBag.Message = TempData["SuccessMessage"];
            return View();
        }


        // GET: Client form
        [HttpGet]
        public IActionResult ClientOrder()
        {
            var products = _orderService.GetProductList();

            var model = new OrderModel
            {
                Drinks = products
                    .Where(p => p.Category.Name == "drink")
                    .Select(p => new SelectListItem
                    {
                        Value = p.Id.ToString(),
                        Text = $"{p.Name} - {p.Price} GEL"
                    }).ToList(),

                 Foods = products
                    .Where(p => p.Category.Name == "food")
                    .Select(p => new SelectListItem
                    {
                        Value = p.Id.ToString(),
                        Text = $"{p.Name} - {p.Price} GEL"
                    }).ToList(),

                 Sweets = products
                    .Where(p => p.Category.Name == "sweet")
                    .Select(p => new SelectListItem
                    {
                        Value = p.Id.ToString(),
                        Text = $"{p.Name} - {p.Price} GEL"
                    }).ToList()
            };

            return View(model);
        }

        // POST: Buy action
        [HttpPost]
        public async Task<IActionResult> ClientOrder(OrderModel model)
        {
            var user = await _orderService.GetLoggedUser();
            model.UserId = user.Id;

            // წაშალე UserId–ის error–ები
            ModelState.Remove("UserId");

            // ModelState–ში ხელით ჩასმა
            //ModelState.SetModelValue("UserId", new ValueProviderResult(model.UserId));
            if (!ModelState.IsValid)
            {
                var products = _orderService.GetProductList();

                model = new OrderModel
                {
                    Drinks = products
                        .Where(p => p.Category.Name == "drink")
                        .Select(p => new SelectListItem
                        {
                            Value = p.Id.ToString(),
                            Text = $"{p.Name} - {p.Price} GEL"
                        }).ToList(),

                    Foods = products
                        .Where(p => p.Category.Name == "food")
                        .Select(p => new SelectListItem
                        {
                            Value = p.Id.ToString(),
                            Text = $"{p.Name} - {p.Price} GEL"
                        }).ToList(),

                    Sweets = products
                        .Where(p => p.Category.Name == "sweet")
                        .Select(p => new SelectListItem
                        {
                            Value = p.Id.ToString(),
                            Text = $"{p.Name} - {p.Price} GEL"
                        }).ToList()
                };
                return View(model);
            }

            // ამოიღე არჩეული პროდუქტები
            var selectedProducts = _orderService.GetChooseProduct(model);

            // დათვალე ჯამი
            var totalAmount = selectedProducts.Sum(p => p.Price);

            // ამოიღე დალოგინებული იუზერი
            //var user = await _orderService.GetLoggedUser();

            //model.UserId = user.Id;
            model.Amount = totalAmount;

            var response = _orderService.CreateOrder(model);

            // ახალი Order
            //var order = new Order
            //{
            //    UserId = user.Id,
            //    Drink = model.Drink,
            //    Food = model.Food,
            //    Sweet = model.Sweet,
            //    Amount = totalAmount,
            //    CreatedDate = DateTime.Now
            //};

            //_context.Orders.Add(order);
            //await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "თქვენი შეკვეთა მიღებულია ✅";
            return RedirectToAction("OrderSuccess");
            //return RedirectToAction("AllOrders");
        }

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

        public async Task<IActionResult> MyOrders(string currentFilter, string searchString, int pageNumber = 1, int pageSize = 5)
        {
            if (!string.IsNullOrEmpty(searchString))
                pageNumber = 1;
            else
                searchString = currentFilter;

            ViewData["CurrentFilter"] = searchString;

            var user = await _orderService.GetLoggedUser();
            var UserId = user.Id;

            var orders = _orderService.GetOrderByUserId(searchString, UserId);

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
            //foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            //{
            //    Console.WriteLine(error.ErrorMessage);
            //}

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
