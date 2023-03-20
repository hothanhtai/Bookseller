using BookSeller.Data;
using BookSeller.Models;
using BookSeller.Data.Cart;
using BookSeller.Data.Service;
using BookSeller.Data.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using BookSeller.Data.Static;

namespace BookSeller.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ShoppingCart _shoppingCart;
        private readonly IOrderService _orderService;       
        public OrdersController(ShoppingCart shoppingCart, AppDbContext context, IOrderService orderService)
        {
            _shoppingCart = shoppingCart;
            _context = context;
            _orderService = orderService;          
        }

        public async Task<IActionResult> Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string userRole = User.FindFirstValue(ClaimTypes.Role);
            var order = await _orderService.GetOrderByUserIdAndRoleAsync(userId,userRole);
            return View(order);
        }
        public async Task<IActionResult> OrderComfirmed()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string userRole = User.FindFirstValue(ClaimTypes.Role);
            var order = await _orderService.GetComfirmedOrderByUserIdAndRoleAsync(userId, userRole);
            return View(order);
        }
        public async Task<IActionResult> OrderDelete()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string userRole = User.FindFirstValue(ClaimTypes.Role);
            var order = await _orderService.GetDeleteOrderByUserIdAndRoleAsync(userId, userRole);
            return View(order);
        }
        public IActionResult ShoppingCart()
        {
            var item = _shoppingCart.GetShoppingCartItems();
            _shoppingCart.ShoppingCartItems = item;
            var response = new ShoppingCartViewModel()
            {
                ShoppingCart = _shoppingCart,
                ShoppingCartTotal = _shoppingCart.GetShoppingCartTotal(),
            };
            return View(response);
        }
        public async Task<IActionResult> AddItemToShoppingCart(int id)
        {
            var item = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .Include(b => b.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if(item != null)
            {
                _shoppingCart.AddItemToCart(item);
            }
            return RedirectToAction(nameof(ShoppingCart));
        }
        public async Task<IActionResult> RemoveItemFromShoppingCart(int id)
        {
            var item = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .Include(b => b.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (item != null)
            {
                _shoppingCart.RemoveItemFromCart(item);
            }
            return RedirectToAction(nameof(ShoppingCart));
        }

        public async Task<IActionResult> CompleteOrder(string name, string address, string phone)
        {
            if(string.IsNullOrEmpty(name) || string.IsNullOrEmpty(address)|| string.IsNullOrEmpty(phone)){
               return RedirectToAction(nameof(Checkout));
            }
            var item = _shoppingCart.GetShoppingCartItems();
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string Name = name;
            string PhoneNumber = phone;
            string Address = address;
            await _orderService.StoreOrderAsync(item, userId, Name, PhoneNumber, Address);
            await _shoppingCart.ClearShoppingCartAsync();
            return View("OrderCompleted");           
        }
        public async Task<IActionResult> Checkout()
        {
            ViewBag.shoppingCart = _shoppingCart.GetShoppingCartItems();
            if (_shoppingCart.GetShoppingCartTotal() == 0)
            {
				TempData["Error"] = "Giỏ hàng trống!!!";
                return RedirectToAction(nameof(ShoppingCart));
			}
            ViewBag.Total = _shoppingCart.GetShoppingCartTotal();
            return View();
        }
        public async Task<IActionResult> Details(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if(order == null)
            {
                return NotFound();
            }
            return View(order);
        }
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> ChangeStt(int OrderId,int stt)
        {
            await _orderService.ChageOrderStatus(OrderId, stt);           
            return RedirectToAction(nameof(Index));
        }
    }
}
