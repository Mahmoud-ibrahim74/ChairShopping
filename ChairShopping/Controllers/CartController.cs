using ChairShopping.Data;
using ChairShopping.Interfaces;
using ChairShopping.Migrations;
using ChairShopping.Models;
using ChairShopping.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ChairShopping.Controllers
{
    public class CartController : Controller
    {
		private readonly ICart _cart;
		private readonly IAdmin _repo;
        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartController(ICart cart,IAdmin repo, AppDbContext db,UserManager<ApplicationUser> userManager)
        {
            _cart = cart;
            _repo=repo;
            _db = db;
           _userManager = userManager;
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
                ViewBag.UserOrder = false;
                ViewBag.Orders = orders;
            }
            var Total = await _cart.TotalOrderPrice(Id);
            ViewBag.Total = Total;
            return View();
        }
        //public async Task<ActionResult<Order>> RemoveOrder(int id)
        //{
        //    if (id != 0)
        //    {
        //        var order = await _repo.GetOrderById(id);
        //        if (order != null)
        //        {
        //            ViewBag.Deleteorder = order;
        //        }
        //    }
        //    return View();
        //}
        [HttpPost]
        public async Task<ActionResult<Order>> RemoveOrder(int id)
        {
            var order =await _repo.GetOrderById(id);
            ViewBag.UserId = order.UserId;
            await _cart.RemoveFromCart(id);
            return RedirectToAction("GetCartById","Cart",new {Id = ViewBag.UserId });
        }
        [HttpPost]
        public async Task<ActionResult<Order>> AddToCart(OrderViewModel model)
        {
            var order =  await _cart.AddToCart(model);
            ViewBag.UserId = order.UserId;
            TempData["cart_added"] = "Product Added Sucessfully";
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public async Task<ActionResult<Order>> UpdateCart(int id)
        {
            var Subproducts = await _repo.GetAllProducts();
            var subUsers = await _repo.GetAllUsers();
            var model = new OrderViewModel
            {
                productList = Subproducts.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.ProductName
                }).ToList(),
                userList = subUsers.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.UserName
                }).ToList()
            };
            if (id > 0)
            {
                var order = await _repo.GetOrderById(id);
                if (order != null)
                {
                    ViewBag.Updateorder = order;
                }
            }
            return View(model);
        }
        [HttpPost]
        public async Task<ActionResult<Order>> UpdateCart(OrderViewModel model, int id)
        {
            var order = await _repo.GetOrderById(id);
            ViewBag.UserId = order.UserId;
            await _repo.EditOrder(model, id);
            return RedirectToAction("GetCartById", "Cart", new { Id = ViewBag.UserId });
        }
        [HttpPost]
        public async Task<ActionResult<Favourite>> AddFavourites(FavouritsViewModel model)
        {
            var favourite = await _cart.AddToFavourite(model);
            ViewBag.UserId = favourite.UserId;
            TempData["cart_added"] = "Product Favourite Added Sucessfully";
            return RedirectToAction("Index", "Home");
        }
    }
}
