using ChairShopping.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ChairShopping.Controllers
{
	public class ShopController : Controller
	{
		private readonly IAdmin _admin;
		public ShopController(IAdmin admin)
		{
			this._admin = admin;
		}

		public IActionResult Index()
		{
			var result = _admin.GetAllProducts().Result.Take(8).ToList();
			return View(result);
		}
	}
}
