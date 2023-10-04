using ChairShopping.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

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
        public async Task<IViewComponentResult> InvokeAsync(int categ_id)
        {

            if (categ_id == 0)  // this condition to load all categories when page is load
                return View("Index", await _productClasses.GetProductsLimitAsync(10));  // view 10 products to prevent load on server
            else
            {
                var res = await _productClasses.GetProductsByCatgoryIdAsync(categ_id);
                return View("Index", await _productClasses.GetProductsByCatgoryIdAsync(categ_id));
            }
        }

    }
}
