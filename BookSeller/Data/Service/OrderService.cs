using BookSeller.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Xml.Linq;

namespace BookSeller.Data.Service
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;
        public OrderService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Order> GetOrderByIdAsync(int Id)
        {
            var order = await _context.Orders.Include(n => n.OrderItems).ThenInclude(n => n.Book).FirstOrDefaultAsync(n => n.Id == Id);
            return order;
        }

        public async Task<List<Order>> GetOrderByUserIdAndRoleAsync(string userId, string userRole)
        {
            var order = await _context.Orders.Include(n => n.OrderItems).ThenInclude(n => n.Book).Include(n => n.User).Where(n => n.Status == 0).ToListAsync();
            if (userRole != "Admin")
            {
                order = order.Where(n => n.UserId == userId).ToList();
            }
            return order;
        }
        public async Task<List<Order>> GetComfirmedOrderByUserIdAndRoleAsync(string userId, string userRole)
        {
            var order = await _context.Orders.Include(n => n.OrderItems).ThenInclude(n => n.Book).Include(n => n.User).Where(n => n.Status == 1).ToListAsync();

            if (userRole != "Admin")
            {
                order = order.Where(n => n.UserId == userId).ToList();
            }
            return order;
        }
        public async Task<List<Order>> GetDeleteOrderByUserIdAndRoleAsync(string userId, string userRole)
        {
            var order = await _context.Orders.Include(n => n.OrderItems).ThenInclude(n => n.Book).Include(n => n.User).Where(n => n.Status == -1).ToListAsync();
            if (userRole != "Admin")
            {
                order = order.Where(n => n.UserId == userId).ToList();
            }
            return order;
        }
        public async Task StoreOrderAsync(List<ShoppingCartItem> items, string userId, string name, string phoneNumber, string address)
        {
            var order = new Order()
            {
                UserId = userId,
                Name = name,
                PhoneNumber = phoneNumber,
                Address = address,
                Status = 0,
                OrderDate = DateTime.Now,
            };
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            foreach (var item in items)
            {
                var orderItem = new OrderItem()
                {
                    Amount = item.Amount,
                    BookId = item.Book.Id,
                    OrderId = order.Id,
                    Price = item.Book.Price
                };
                await _context.OrderItems.AddAsync(orderItem);
            }

            await _context.SaveChangesAsync();
        }       
        public async Task ChageOrderStatus(int Id, int stt)
        {
            var order = await _context.Orders.Include(n => n.OrderItems).ThenInclude(n => n.Book).FirstOrDefaultAsync(n => n.Id == Id);
            order.Status = stt;
            _context.Update(order);
            await _context.SaveChangesAsync();
        }
    }
}