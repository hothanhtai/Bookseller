using BookSeller.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace BookSeller.Data.Service
{
	public interface IOrderService
	{
		Task StoreOrderAsync(List<ShoppingCartItem> items, string userId, string name, string phoneNumber, string address);
	
		Task<List<Order>> GetOrderByUserIdAndRoleAsync(string userId,string userRole);

        Task<List<Order>> GetComfirmedOrderByUserIdAndRoleAsync(string userId, string userRole);

        Task<List<Order>> GetDeleteOrderByUserIdAndRoleAsync(string userId, string userRole);

        Task<Order> GetOrderByIdAsync(int Id);

		Task ChageOrderStatus(int Id,int stt);
    }
}
