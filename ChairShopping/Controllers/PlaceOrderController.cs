using ChairShopping.Data;
using ChairShopping.Interfaces;
using ChairShopping.Models;
using ChairShopping.Repositories;
using ChairShopping.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ChairShopping.Controllers
{
    public class PlaceOrderController : Controller
    {
		private readonly SignInManager<ApplicationUser> _SignInManager;
		private readonly UserManager<ApplicationUser> _UserManager;
		private readonly ICart _cart;
		private readonly AppDbContext _db;
		private readonly IAdmin _repo;

		public PlaceOrderController(ICart cart,AppDbContext db,IAdmin repo, SignInManager<ApplicationUser> SignInManager, UserManager<ApplicationUser> UserManager)
        {
            _db = db;
            _repo = repo;
            _SignInManager = SignInManager;
            _UserManager = UserManager;
            _cart = cart;
		}
		[HttpGet]
        public async Task<IActionResult> PlaceOrder()
        {
			if (_SignInManager.IsSignedIn(User))
			{
				var user = await _UserManager.GetUserAsync(User);
				ViewBag.user = user;
			}
			var orders = await _repo.GetAllOrders();
			foreach (var order in orders)
			{
				if (_SignInManager.IsSignedIn(User))
				{
					var user = await _UserManager.GetUserAsync(User);
					if (user.Id == order.UserId)
					{
						ViewBag.order = orders;
					}
					var Total = await _cart.TotalOrderPrice(user.Id);
					ViewBag.total = Total;
				}
			}
			var coupons = await _repo.GetAllCoupons();
			if (coupons.Count()==0)
			{
				ViewBag.c = false;
			}
			else
			{
				ViewBag.c = true;
			}
			return View();
        }
        [HttpPost]
        public async Task<IActionResult> PlaceOrder(PlaceOrderViewModel model)
        {
			var coupons = await _repo.GetAllCoupons();
			if (coupons.Count()!=0)
			{
				foreach (var coupon in coupons)
				{
					if (coupon.CouponCode == model.Coupon.CouponCode)
					{
						model.CouponId = coupon.Id;
					}
					else
					{
						ModelState.AddModelError("", "Invalid Coupon");
					}
				}
			}
			if (ModelState.IsValid)
            {
				var orderPlace = new PlaceOrder
                {
                    Address = model.Address,
                    CouponId = model.CouponId,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    OrderNotes = model.OrderNotes,
                    Phone = model.Phone
                };
                await _db.placeOrders.AddAsync(orderPlace);
                await _db.SaveChangesAsync();
				if (_SignInManager.IsSignedIn(User))
				{
					var orders = await _repo.GetAllOrders();
					foreach (var order in orders)
					{
						var user = await _UserManager.GetUserAsync(User);
						if (order.UserId==user.Id)
						{
							await _repo.DeleteOrder(order.Id);
							await _db.SaveChangesAsync();
						}
					}
				}
				return RedirectToAction("ConfirmOrder");
			}
            return View(model);
        }
        public IActionResult ConfirmOrder()
        {
            return View();
        }

	}
}
