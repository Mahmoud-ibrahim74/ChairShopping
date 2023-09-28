using ChairShopping.Data;
using ChairShopping.Interfaces;
using ChairShopping.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ChairShopping.Repositories
{
    public class Admin : IAdmin
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly AppDbContext _db;


        public Admin(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, AppDbContext db, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _db = db;
        }
        public async Task<IEnumerable<ApplicationUser>> GetAllUsers()
        {
            var users = await _db.Users.ToListAsync();
            if (users == null)
            {
                return null;
            }
            return users;
        }
        public async Task<ApplicationUser> AddUser(RegisterVM model)
        {
            if (model == null)
            {
                return null;
            }
            var exist = await _userManager.FindByEmailAsync(model.Email);
            if (exist != null)
            {
                return null;
            }
            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                IsAgree = model.IsAgree               
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return user;
            }
            return null;
        }

        public async Task<ApplicationUser> EditUser(RegisterVM model, string id)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                return null;
            }
            user.UserName = model.UserName;
            user.Email = model.Email;
            user.PasswordHash = model.Password;
            user.IsAgree = model.IsAgree;
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
            return user;
        }

        public Task<ApplicationUser> GetUserById(string id)
        {
            return _db.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<ApplicationUser> DeleteUser(string id)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                return null;
            }
            _db.Users.Remove(user);
            await _db.SaveChangesAsync();
            return user;
        }
    }
}
