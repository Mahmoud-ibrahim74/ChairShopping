using Microsoft.AspNetCore.Identity;

namespace ChairShopping.Data
{
    public class ApplicationUser:IdentityUser
    {
        public string? Country { get; set; }
    }
}
