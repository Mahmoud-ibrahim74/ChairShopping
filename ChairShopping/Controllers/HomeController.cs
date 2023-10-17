using ChairShopping.Interfaces;
using ChairShopping.Models;
using ChairShopping.Repositories;
using ChairShopping.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ChairShopping.Controllers
{
    [Authorize]  // use this to force user to login if session is expired
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAdmin _repo;
        private readonly ICart _cart;

        public HomeController(ICart cart,ILogger<HomeController> logger,IAdmin repo)
        {
            _logger = logger;
            _repo = repo;
            _cart = cart;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult<List<Product>>> Search(string search)
        {
            var products = await _repo.SearchProduct(search);
            if (products == null)
            {
                return null;
            }
            return ViewComponent("ProductClasses", products);
        }
        [HttpPost]
        public async Task<ActionResult<Order>> RemoveOrder(int id)
        {
            var order = await _repo.GetOrderById(id);
            var orders = await _cart.GetCartById(order.UserId);
            ViewBag.UserId = order.UserId;
            await _cart.RemoveFromCart(id);
            return Json(new { success = true , cartCount = orders.Count()-1 , Orderid = order.Id});
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpPost]
        public IActionResult ProductClasses(string categ_id)
        {
            int cast_id = Convert.ToInt32(categ_id);
            return ViewComponent("ProductClasses",cast_id);
        }
    }
}