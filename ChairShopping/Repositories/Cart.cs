using ChairShopping.Data;
using ChairShopping.Interfaces;
using ChairShopping.Models;
using ChairShopping.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace ChairShopping.Repositories
{
	public class Cart : ICart
	{
		private readonly AppDbContext _db;
        private readonly IAdmin _repo;

        public Cart(AppDbContext db,IAdmin repo)
        {
			_db = db;
			_repo = repo;
        }

		public decimal TotalPrice { get; set; } = 0;

		public Task<Order> AddToCart(OrderViewModel model)
		{
			throw new NotImplementedException();
		}

		public async Task<IEnumerable<Order>> GetAllCarts()
		{
			var orders= await _db.orders.Include(x => x.Product).Include(x => x.User).ToListAsync();
			return orders;
		}
		public async Task<IEnumerable<Order>> GetCartById(string id)
		{
			if (string.IsNullOrEmpty(id))
			{
				return null;
			}
			List<Order> orders = await _db.orders.Where(x => x.UserId == id).Include(x => x.Product).Include(x => x.User).ToListAsync();
			return orders;
		}
		public async Task<Order> RemoveFromCart(int id)
		{
			var order = await _repo.GetOrderById(id);
			if (order == null)
			{
				return null;
			}
			 _db.orders.Remove(order);
			await _db.SaveChangesAsync();
			return order;
		}
		public async Task<decimal> TotalOrderPrice(string Id)
		{
			var orders = await GetCartById(Id);
			foreach (var item in orders)
			{
				decimal SubTotal = item.TotalPrice;
				TotalPrice = TotalPrice + SubTotal;
			}
			return TotalPrice;
		}
	}
}
