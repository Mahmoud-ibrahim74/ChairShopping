using ChairShopping.Data;
using ChairShopping.Interfaces;
using ChairShopping.Migrations;
using ChairShopping.Models;
using ChairShopping.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChairShopping.Controllers
{
    public class CartController : Controller
    {
		private readonly ICart _cart;
		private readonly IAdmin _repo;
        private readonly AppDbContext _db;

        public CartController(ICart cart,IAdmin repo, AppDbContext db)
        {
            _cart = cart;
            _repo=repo;
            _db = db;
        }
        public async Task<IActionResult> GetCartById(string Id)
        {
            var user = await _repo.GetUserById(Id);
            if (user == null)
            {
                return null;
            }
            var orders =await _cart.GetCartById(Id);
            var user_Order = await _db.orders.Where(x => x.UserId == user.Id).ToListAsync();
            if (orders.Count()==0 && user_Order.Count()==0)
            {
                ViewBag.UserOrder = true;
            }
            else
            {
                ViewBag.Orders = orders;
            }
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
        [HttpGet]
        public async Task<ActionResult<Order>> AddToCart(OrderViewModel model)
        {
            var order =  await _cart.AddToCart(model);
            ViewBag.UserId = order.UserId;
            //return RedirectToAction("GetCartById", "Cart", new { Id = ViewBag.UserId });
            return RedirectToAction("Index", "Home");
        }
        //[HttpPost]
        //public async Task<IActionResult> Coupon(CouponViewModel model)
        //{
        //    //coupons , modelcoupon
        //    var coupons = await _repo.GetAllCoupons();
        //    foreach (var item in coupons)
        //    {
        //        if (model.CouponCode==item.CouponCode)
        //        {
        //            return RedirectToAction("GetCartById", "Cart");
        //        }
        //        else
        //        {
        //            return RedirectToAction("Index", "Home");
        //        }
        //    }
        //}
    }
}
