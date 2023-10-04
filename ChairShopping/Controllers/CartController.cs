using ChairShopping.Interfaces;
using ChairShopping.Migrations;
using ChairShopping.Models;
using ChairShopping.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace ChairShopping.Controllers
{
    public class CartController : Controller
    {
		private readonly ICart _cart;
		private readonly IAdmin _repo;

		public CartController(ICart cart,IAdmin repo)
        {
            _cart = cart;
            _repo=repo;
        }
        public async Task<IActionResult> GetCartById(string Id)
        {
            var user = await _repo.GetUserById(Id);
            if (user == null)
            {
                return null;
            }
            var orders =await _cart.GetCartById(Id);
            ViewBag.Orders = orders;
            var Total = await _cart.TotalOrderPrice(Id);
            ViewBag.Total = Total;
            return View();
        }
        public async Task<ActionResult<Order>> RemoveOrder(int id)
        {
            if (id > 0)
            {
                var order = await _repo.GetOrderById(id);
                if (order != null)
                {
                    ViewBag.Deleteorder = order;
                }
            }
            return View();
        }
        [HttpPost]
        public async Task<ActionResult<Order>> RemoveOrder(int id, OrderViewModel model)
        {
            var order =await _repo.GetOrderById(id);
            ViewBag.UserId = order.UserId;
            await _cart.RemoveFromCart(id);
            return RedirectToAction("GetCartById","Cart",new {Id = ViewBag.UserId });
        }
    }
}
