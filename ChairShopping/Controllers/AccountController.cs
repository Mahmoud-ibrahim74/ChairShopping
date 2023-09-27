using ChairShopping.Data;
using ChairShopping.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using NuGet.Protocol.Plugins;

namespace ChairShopping.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM login)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(login.Email);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, login.Password, login.RememberMe, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Cannot Login Please try Again");
                    }
                }
           
            }
            // If we reach this point, something went wrong, so redisplay the login form
            return View(login);
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM register)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(register.Email);
                if (user == null)
                {
                    var NewUser = new ApplicationUser
                    {
                        UserName = register.UserName,
                        Email = register.Email,
                        IsAgree = register.IsAgree,

                    };
                   var result = await _userManager.CreateAsync(NewUser,register.Password);
                    if (result.Succeeded)
                    {
                        ViewData["UserAdded"] = "User Added Sucessfully";
                        return RedirectToAction("Index", "Home");                        
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Cannot Login Please try Again");
                    }
                }
            }
            // If we reach this point, something went wrong, so redisplay the login form
            return View(register);
        }



    }
}
