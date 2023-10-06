using ChairShopping.Models;

namespace ChairShopping.ViewModel
{
	public class PlaceOrderViewModel
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Address { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public string OrderNotes { get; set; }
		public int CouponId { get; set; }
		public Coupon? Coupon { get; set; }
	}
}
