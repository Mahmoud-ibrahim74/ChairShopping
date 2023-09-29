using ChairShopping.Data;
using ChairShopping.Interfaces;
using ChairShopping.Models;
using ChairShopping.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

        public async Task<ApplicationUser> GetUserById(string id)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Id == id);
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

        public async Task<IEnumerable<ApplicationRole>> GetAllRoles()
        {
            var roles = await _db.Roles.ToListAsync();
            if (roles == null)
            {
                return null;
            }
            return roles;
        }

        public async Task<ApplicationRole> AddRole(ApplicationRole model)
        {
            if (model == null)
            {
                return null;
            }
            var exist = await _roleManager.FindByNameAsync(model.Name);
            if (exist != null)
            {
                return null;
            }
            var role = new ApplicationRole
            {
                Name = model.Name,
                ConcurrencyStamp = Guid.NewGuid().ToString()
        };
            var result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                return role;
            }
            return null;
        }

        public async Task<ApplicationRole> EditRole(ApplicationRole model, string id)
        {
            var role = await _db.Roles.FirstOrDefaultAsync(x => x.Id == id);
            if (role == null)
            {
                return null;
            }
            role.Name = model.Name;
            _db.Roles.Update(role);
            await _db.SaveChangesAsync();
            return role;
        }

        public async Task<ApplicationRole> GetRoleById(string id)
        {
            return await _db.Roles.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<ApplicationRole> DeleteRole(string id)
        {
            var role = await _db.Roles.FirstOrDefaultAsync(x => x.Id == id);
            if (role == null)
            {
                return null;
            }
            _db.Roles.Remove(role);
            await _db.SaveChangesAsync();
            return role;
        }

        public async Task<IEnumerable<UserRoleViewModel>> GetAllUserRole()
        {
            var query = await (
          from userRole in _db.UserRoles
          join users in _db.Users
          on userRole.UserId equals users.Id
          join roles in _db.Roles
          on userRole.RoleId equals roles.Id
          select new
          {
              userRole.UserId,
              users.UserName,
              userRole.RoleId,
              roles.Name
          }).ToListAsync();
            List<UserRoleViewModel> userRolesModels = new List<UserRoleViewModel>();
            if (query.Count > 0)
            {
                for (int i = 0; i < query.Count; i++)
                {
                    var model = new UserRoleViewModel
                    {
                        RoleId = query[i].RoleId,
                        UserId = query[i].UserId,
                        RoleName = query[i].Name,
                        UserName = query[i].UserName
                    };
                    userRolesModels.Add(model);
                }
            }
            return userRolesModels;
        }
        public async Task<UserRoleViewModel> AddUserRole(UserRoleViewModel model)
        {
            if (model == null)
            {
                return null;
            }
            var user = await _userManager.FindByNameAsync(model.UserName);
            var role = await _roleManager.FindByNameAsync(model.RoleName);
            if (user != null && role != null)
            {
                await _userManager.AddToRoleAsync(user, role.Name);
            }
            return null;
        }
        public async Task<UserRoleViewModel> EditUserRole(UserRoleViewModel model)
        {
            //Useradmin has id , id of Useradmin has roleid , roleid has name , i need to change this name 
            if (model.UserId == null)
            {
                return null;
            }
            var user = await _userManager.FindByIdAsync(model.UserId);
            var currentRole = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
            var newRole = await _roleManager.FindByNameAsync(model.RoleName);
            if (user != null && currentRole != null && newRole != null)
            {
                var result = await _userManager.RemoveFromRoleAsync(user, currentRole);
                if (result.Succeeded)
                {
                    result = await _userManager.AddToRoleAsync(user, newRole.Name);
                }
            }
            return model;
        }

        public async Task<UserRoleViewModel> DeleteUserRole(UserRoleViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
            if (user != null && role != null)
            {
                await _userManager.RemoveFromRoleAsync(user, role);
            }
            return null;
        }

        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            return await _db.categories.ToListAsync();
        }

        public async Task<Category> AddCategory(Category model)
        {
            if (model == null)
            {
                return null;
            }
            var category = await _db.categories.FirstOrDefaultAsync(x => x.CategoryName == model.CategoryName);
            if (category != null)
            {
                return null;
            }
            var newCat = new Category
            {
                CategoryName = model.CategoryName
            };
            await _db.categories.AddAsync(newCat);
            await _db.SaveChangesAsync();
            return category;
        }

        public async Task<Category> EditCategory(Category model, int id)
        {
            if (id < 0 && model == null)
            {
                return null;
            }
            var category = await GetCategoryById(id);
            if (category==null)
            {
                return null;
            }
            category.CategoryName = model.CategoryName;
             _db.categories.Update(category);
            await _db.SaveChangesAsync();
            return category;
        }

        public async Task<Category> GetCategoryById(int id)
        {
            var category = await _db.categories.FirstOrDefaultAsync(x => x.Id == id);
            if (category == null)
            {
                return null;
            }
            return category;
        }

        public async Task<Category> DeleteCategory(int id)
        {
            var category = await GetCategoryById(id);
            if (category == null)
            {
                return null;
            }
            _db.categories.Remove(category);
            await _db.SaveChangesAsync();
            return category;
        }
    }
}
