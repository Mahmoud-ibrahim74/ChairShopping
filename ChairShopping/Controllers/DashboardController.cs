using ChairShopping.Data;
using ChairShopping.Interfaces;
using ChairShopping.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChairShopping.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
	{
        private readonly IAdmin _repo;

        public DashboardController(IAdmin repo)
        {
			_repo = repo;
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
	
	}
}
