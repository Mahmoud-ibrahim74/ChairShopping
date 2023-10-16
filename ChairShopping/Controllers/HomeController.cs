using ChairShopping.Interfaces;
using ChairShopping.Models;
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

        public HomeController(ILogger<HomeController> logger,IAdmin repo)
        {
            _logger = logger;
            _repo = repo;
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