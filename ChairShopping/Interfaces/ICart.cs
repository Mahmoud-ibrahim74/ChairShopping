﻿using ChairShopping.Models;
using ChairShopping.ViewModel;

namespace ChairShopping.Interfaces
{
    public interface ICart
    {
        public decimal TotalPrice { get; set; }
        public  Task<IEnumerable<Order>> GetAllCarts();
        public Task<IEnumerable<Order>> GetCartById(string id);
	    public Task<Order> AddToCart(OrderViewModel model);
        public Task<Order> RemoveFromCart(int id);

        public Task<decimal> TotalOrderPrice(string Id);

    }
}