using ChairShopping.Data;
using ChairShopping.Interfaces;
using ChairShopping.Models;
using Microsoft.AspNetCore.Mvc;

namespace ChairShopping.ComponentsControllers
{
    [ViewComponent(Name = "ProductClasses")]
    public class ProductClasses : ViewComponent
    {
        private readonly IAdmin _productClasses;
        public ProductClasses(IAdmin productClasses)
        {
            this._productClasses = productClasses;
        }
        public async Task<IViewComponentResult> InvokeAsync(int categ_id,int PageNumber = 1,int PageSize = 8)
        {

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
