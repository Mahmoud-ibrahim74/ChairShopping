using ChairShopping.Data;
using ChairShopping.Interfaces;
using ChairShopping.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ChairShopping.ComponentsControllers
{
    [ViewComponent(Name = "ProductClasses")]
    public class ProductClasses : ViewComponent
    {
        private readonly IAdmin _productClasses;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ICart _cart;

        public ProductClasses(IAdmin productClasses,UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,ICart cart)
        {
            _signInManager=signInManager;
            this._productClasses = productClasses;
            _userManager = userManager;
            _cart = cart;
        }
        public async Task<IViewComponentResult> InvokeAsync(int categ_id,int PageNumber = 1,int PageSize = 8)
        {
            //var favs = await _productClasses.GetAllFavourits();
            //foreach (var item in favs)
            //{
            //    ViewBag.favUser = item;
            //    if (item.IsFavourite==true)
            //    {
            //        ViewBag.fav = true;
            //    }
            //    else
            //    {
            //        ViewBag.fav = false;
            //    }
            //}
            if (categ_id == 0)  // this condition to load all categories when page is load
            {
                return View("Index", await _productClasses.GetProductsLimitAsync(10));  // view 10 products to prevent load on server

            }
            else
            {
                return View("Index", await _productClasses.GetProductsByCatgoryIdAsync(categ_id));
            }
        }

    }
}
