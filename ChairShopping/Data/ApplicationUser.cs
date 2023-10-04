using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ChairShopping.Data
{
    public class ApplicationUser:IdentityUser
    {
        public string Country { get; set; } = "Egypt";
        public bool IsAgree { get; set; }
        public string ImageUrl { get; set; }
    }
}
