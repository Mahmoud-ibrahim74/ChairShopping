using Microsoft.AspNetCore.Mvc;

namespace ChairShopping.Controllers
{
	public class DashboardController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
