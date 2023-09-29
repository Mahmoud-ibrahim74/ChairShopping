using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ChairShopping.ViewModel
{
    public class ForgetPasswordVM:IdentityUser
    {
        [Required]
        public string Email { get; set; }
    }
}
