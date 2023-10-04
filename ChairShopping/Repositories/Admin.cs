using ChairShopping.Data;
using ChairShopping.Interfaces;
using ChairShopping.Models;
using ChairShopping.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;
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

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _db.products.Include(x => x.Category).ToListAsync();
        }

        public async Task<Product> AddProduct(ProductViewModel model)
        {
            if (model == null)
            {
                return null;
            }
            var product = await _db.products.Include(x=>x.Category).FirstOrDefaultAsync(x => x.ProductName == model.ProductName);
            if (product != null)
            {
                return null;
            }
            var folderPath = Directory.GetCurrentDirectory() + "/wwwroot/asset/images";
            var folderName = Path.GetFileName(model.Image.FileName);
            var finalPath = Path.Combine(folderPath, folderName);
            using (var stream = new FileStream(finalPath, FileMode.Create))
            {
                await model.Image.CopyToAsync(stream);
            }
            var newprd = new Product
            {
                ProductName = model.ProductName,
                Color = model.Color,
                Price = model.Price,
                ProductDescription = model.ProductDescription,
                ProductCreation = model.ProductCreation,
                Image = model.Image.FileName,
                CategoryId = model.CategoryId
            };
            await _db.products.AddAsync(newprd);
            await _db.SaveChangesAsync();
            return newprd;
        }

        public async Task<Product> EditProduct(ProductViewModel model, int id)
        {
            if (id < 0 && model == null)
            {
                return null;
            }
            var prd = await GetProductById(id);
            if (prd == null)
            {
                return null;
            }
            var folderPath = Directory.GetCurrentDirectory() + "/wwwroot/asset/images";
            var folderName = Path.GetFileName(model.Image.FileName);
            var finalPath = Path.Combine(folderPath, folderName);
            using (var stream = new FileStream(finalPath, FileMode.Create))
            {
                await model.Image.CopyToAsync(stream);
            }
            prd.ProductDescription = model.ProductDescription;
            prd.ProductCreation = model.ProductCreation;
            prd.ProductName = model.ProductName;
            prd.Price = model.Price;
            prd.Color = model.Color;
            prd.Image = model.Image.FileName;
            prd.CategoryId = model.CategoryId;
            _db.products.Update(prd);
            await _db.SaveChangesAsync();
            return prd;
        }

        public async Task<Product> GetProductById(int id)
        {
            var prd = await _db.products.Include(x=>x.Category).FirstOrDefaultAsync(x => x.Id == id);
            if (prd == null)
            {
                return null;
            }
            return prd;
        }

        public async Task<Product> DeleteProduct(int id)
        {
            var prd = await GetProductById(id);
            if (prd == null)
            {
                return null;
            }
            _db.products.Remove(prd);
            await _db.SaveChangesAsync();
            return prd;
        }

        public async Task<IEnumerable<Order>> GetAllOrders()
        {
            return await _db.orders.Include(x => x.User).Include(x => x.Product).ThenInclude(x => x.Category).ToListAsync();
        }

        public async Task<Order> AddOrder(OrderViewModel model)
        {
            if (model == null)
            {
                return null;
            }
            model.TotalPrice = (model.Price * model.Quantity) - model.Discount;
            var order = new Order
            {
                Discount = model.Discount,
                Price = model.Price,
                Quantity = model.Quantity,
                ProductId = model.ProductId,
                UserId = model.UserId,
                OrderDate = model.OrderDate,
                TotalPrice = model.TotalPrice
            };
            await _db.orders.AddAsync(order);
            await _db.SaveChangesAsync();
            return order;
        }

        public async Task<Order> EditOrder(OrderViewModel model, int id)
        {
            if (id < 0 && model == null)
            {
                return null;
            }
            var order = await GetOrderById(id);
            if (order == null)
            {
                return null;
            }
            model.TotalPrice = (model.Price * model.Quantity) - model.Discount;
            order.OrderDate = model.OrderDate;
            order.Price = model.Price;
            order.Quantity = model.Quantity;
            order.ProductId = model.ProductId;
            order.UserId = model.UserId;
            order.Discount = model.Discount;
            order.TotalPrice = model.TotalPrice;
            _db.orders.Update(order);
            await _db.SaveChangesAsync();
            return order;
        }

        public async Task<Order> GetOrderById(int id)
        {
            var order = await _db.orders.Include(x => x.User).Include(x=>x.Product).ThenInclude(x=>x.Category).FirstOrDefaultAsync(x => x.Id == id);
            if (order == null)
            {
                return null;
            }
            return order;
        }

        public async Task<Order> DeleteOrder(int id)
        {
            var order = await GetOrderById(id);
            if (order == null)
            {
                return null;
            }
            _db.orders.Remove(order);
            await _db.SaveChangesAsync();
            return order;
        }

        public async Task<List<Category>> GetCategoriesLimitAsync(int limit)
        {
            var categ_limit = await _db.categories.Take(limit).ToListAsync();
            if (categ_limit == null)
            {
                return null;
            }
            return categ_limit;
        }

        public async Task<List<Product>> GetProductsLimitAsync(int limit)
        {
            var prod_limit = await _db.products.Take(limit).ToListAsync();
            if (prod_limit == null)
            {
                return null;
            }
            return prod_limit;
        }
        [Obsolete("Notes This Method Return Only 10 Products")]
        public async Task<List<Product>> GetProductsByCatgoryIdAsync(int id)
        {
            var _products = await _db.products.Where(x => x.CategoryId == id).Take(10).ToListAsync();
            if (_products == null)
            {
                return null;
            }
            return _products;
        }
    }
}
