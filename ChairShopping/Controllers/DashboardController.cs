using ChairShopping.Data;
using ChairShopping.Interfaces;
using ChairShopping.Models;
using ChairShopping.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChairShopping.Controllers
{
	public class DashboardController : Controller
	{
        private readonly IAdmin _repo;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public DashboardController(IAdmin repo, RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager)
        {
			_repo = repo;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public IActionResult Index()
		{
			return View();
		}
		public async Task<IActionResult> GetAllUsers()
		{
			ViewBag.AllUsers =await _repo.GetAllUsers();
			return View();
		}
        public IActionResult AddUser()
        {
            return View();
        }
		[HttpPost]
		public async Task<ActionResult<ApplicationUser>> AddUser(RegisterVM model)
		{
			if (ModelState.IsValid)
			{
				await _repo.AddUser(model);
				return RedirectToAction("GetAllUsers");
			}
            else
            {
                ModelState.AddModelError("Key", "something wrong in validation");
            }
			return View();
		}
        public async Task<ActionResult<ApplicationUser>> EditUser(string id)
        {
            if (id != null)
            {
                var user = await _repo.GetUserById(id);
                if (user != null)
                {
                    ViewBag.UpdateUser = user;
                }
            }
            return View();
        }
        [HttpPost]
        public async Task<ActionResult<ApplicationUser>> EditUser(RegisterVM model, string id)
        {
            await _repo.EditUser(model, id);
            return RedirectToAction("GetAllUsers", "Dashboard");
        }
        public async Task<ActionResult> DeleteUser(string id)
        {
            if (id != null)
            {
                var user = await _repo.GetUserById(id);
                if (user != null)
                {
                    ViewBag.UpdateUser = user;
                }
            }
            return View();
        }
        [HttpPost]
        public async Task<ActionResult<ApplicationUser>> DeleteUser(string id, RegisterVM model)
        {
            await _repo.DeleteUser(id);
            return RedirectToAction("GetAllUsers", "Dashboard");
        }
        public async Task<ActionResult<ApplicationUser>> GetUserDetails(string id)
        {
            if (id != null)
            {
                var user = await _repo.GetUserById(id);
                if (user != null)
                {
                    ViewBag.UpdateUser = user;
                }
            }
            return View();
        }
        ///////////////////////////////////////////////////////////////////////////////
        public async Task<IActionResult> GetAllRoles()
        {
            ViewBag.AllRoles = await _repo.GetAllRoles();
            return View();
        }
        public IActionResult AddRole()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult<ApplicationRole>> AddRole(ApplicationRole model)
        {
            if (ModelState.IsValid)
            {
                await _repo.AddRole(model);
                return RedirectToAction("GetAllRoles");
            }
            return View();
        }
        public async Task<ActionResult<ApplicationRole>> EditRole(string id)
        {
            if (id != null)
            {
                var role = await _repo.GetRoleById(id);
                if (role != null)
                {
                    ViewBag.Updaterole = role;
                }
            }
            return View();
        }
        [HttpPost]
        public async Task<ActionResult<ApplicationRole>> EditRole(ApplicationRole model, string id)
        {
            await _repo.EditRole(model, id);
            return RedirectToAction("GetAllRoles", "Dashboard");
        }
        public async Task<ActionResult> DeleteRole(string id)
        {
            if (id != null)
            {
                var role = await _repo.GetRoleById(id);
                if (role != null)
                {
                    ViewBag.Updaterole = role;
                }
            }
            return View();
        }
        [HttpPost]
        public async Task<ActionResult<ApplicationRole>> DeleteRole(string id, ApplicationRole model)
        {
            await _repo.DeleteRole(id);
            return RedirectToAction("GetAllRoles", "Dashboard");
        }
        public async Task<ActionResult<ApplicationRole>> GetRoleDetails(string id)
        {
            if (id != null)
            {
                var role = await _repo.GetRoleById(id);
                if (role != null)
                {
                    ViewBag.Updaterole = role;
                }
            }
            return View();
        }
        /////////////////////////////////////////////////////////
        public async Task<ActionResult<IEnumerable<UserRoleViewModel>>> UserRoles()
        {
            var userRoles = await _repo.GetAllUserRole();
            if (userRoles != null)
            {
                ViewBag.userRoles = userRoles;
            }
            return View();
        }
        public async Task<IActionResult> AddUserRole()
        {
            var model = new UserRoleViewModel
            {
                Roles = await _roleManager.Roles.ToListAsync(),
                Users = await _repo.GetAllUsers()
            };
            return View(model);
        }
        [HttpPost]
        public async Task<ActionResult<UserRoleViewModel>> AddUserRole(UserRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _repo.AddUserRole(model);
                return RedirectToAction("UserRoles");
            }
            return View();
        }
        public async Task<IActionResult> EditUserRole(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            ViewBag.userRole = user;
            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var model = new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    RoleName = userRoles.FirstOrDefault(),
                    Roles = _roleManager.Roles.ToList()
                };
                return View(model);
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<ActionResult<UserRoleViewModel>> EditUserRole(UserRoleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            await _repo.EditUserRole(model);
            return RedirectToAction("UserRoles");
        }
        public async Task<IActionResult> DeleteUserRole(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            ViewBag.userRoleDelete = user;
            if (user != null)
            {
                var model = new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };
                return View(model);
            }

            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteUserRole(UserRoleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            await _repo.DeleteUserRole(model);
            return RedirectToAction("UserRoles");
        }
        ///////////////////////////////////////////////////////////////////
        public async Task<IActionResult> GetAllCategories()
        {
            ViewBag.AllCategories = await _repo.GetAllCategories();
            return View();
        }
        public IActionResult AddCategory()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult<Category>> AddCategory(Category model)
        {
            if (ModelState.IsValid)
            {
                await _repo.AddCategory(model);
                return RedirectToAction("GetAllCategories");
            }
            else
            {
                ModelState.AddModelError("Key", "something wrong in validation");
            }
            return View();
        }
        public async Task<ActionResult<Category>> EditCategory(int id)
        {
            if (id > 0)
            {
                var Category = await _repo.GetCategoryById(id);
                if (Category != null)
                {
                    ViewBag.UpdateCategory = Category;
                }
            }
            return View();
        }
        [HttpPost]
        public async Task<ActionResult<Category>> EditCategory(Category model, int id)
        {
            await _repo.EditCategory(model, id);
            return RedirectToAction("GetAllCategories", "Dashboard");
        }
        public async Task<ActionResult<Category>> DeleteCategory(int id)
        {
            if (id > 0)
            {
                var cat = await _repo.GetCategoryById(id);
                if (cat != null)
                {
                    ViewBag.Updatecat = cat;
                }
            }
            return View();
        }
        [HttpPost]
        public async Task<ActionResult<Category>> DeleteCategory(int id, Category model)
        {
            await _repo.DeleteCategory(id);
            return RedirectToAction("GetAllCategories", "Dashboard");
        }
        public async Task<ActionResult<Category>> GetCategoryDetails(int id)
        {
            if (id > 0)
            {
                var cat = await _repo.GetCategoryById(id);
                if (cat != null)
                {
                    ViewBag.Updatecat = cat;
                }
            }
            return View();
        }
        ///////////////////////////////////////////////////////////////////////////////////////////
        
    }
}

