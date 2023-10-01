using ChairShopping.Data;
using ChairShopping.Interfaces;
using ChairShopping.Repositories;
using ChairShopping.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ChairShopping.Controllers
{
	public class AccountController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly RoleManager<ApplicationRole> _roleManager;
		public AccountController(UserManager<ApplicationUser> userManager,
			SignInManager<ApplicationUser> signInManager,
			RoleManager<ApplicationRole> roleManager)
		{
			this._userManager = userManager;
			this._signInManager = signInManager;
			_roleManager = roleManager;
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
					var result = await _userManager.CreateAsync(NewUser, register.Password);
					var roleResult = await _userManager.AddToRoleAsync(NewUser, "User");
					if (result.Succeeded && roleResult.Succeeded)
					{
						TempData["UserAdded"] = "User Added Sucessfully";
						return RedirectToAction("Login", "Account");
					}
					else
					{
						foreach (var err in result.Errors)
						{
							ModelState.AddModelError(string.Empty, err.Description); // handle errors
						}
					}
				}
				else
				{
					ModelState.AddModelError(string.Empty, "User Already Exist !!");
				}
			}
			// If we reach this point, something went wrong, so redisplay the login form
			return View(register);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
			return RedirectToAction("Login", "Account");
		}
		public IActionResult ForgetPassword()
		{
			return View();
		}
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ForgetPassword(ForgetPasswordVM forgetPassword)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByEmailAsync(forgetPassword.Email);
				if (user != null)
				{
					var token = await _userManager.GeneratePasswordResetTokenAsync(user);
					var callbackUrl = Url.Action("ResetPassword", "Account",
						new { userId = user.Id, token = token }, Request.Scheme);

					// Send the password reset link to the user's email
					// You can use any email sending mechanism here
					IEmailSender _emailSender = new GmailEmailSender();
					await _emailSender.SendEmailAsync(forgetPassword.Email, "Reset Password",
						$"Please reset your password by clicking <a href='{callbackUrl}'>here</a>.");

					return RedirectToAction("ForgotPasswordConfirmation");
				}
				else
				{
					ModelState.AddModelError(string.Empty, "Email doesn't  Exist !!");
				}
                    return RedirectToAction("ForgotPasswordConfirmation");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Email doesn't  Exist !!");
                }
            

			// If we got this far, something failed, redisplay the form
			return View(forgetPassword);
		}

		public IActionResult ResetPassword()
		{
			return View();
		}


	}
}
