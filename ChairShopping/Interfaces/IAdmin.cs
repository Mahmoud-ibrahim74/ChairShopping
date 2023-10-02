using ChairShopping.Data;
using ChairShopping.Models;
using ChairShopping.ViewModel;

namespace ChairShopping.Interfaces
{
    public interface IAdmin
    {
        public Task<IEnumerable<ApplicationUser>> GetAllUsers();
        public Task<ApplicationUser> AddUser(RegisterVM model);
        public Task<ApplicationUser> EditUser(RegisterVM model,string id);
        public Task<ApplicationUser> GetUserById(string id);
        public Task<ApplicationUser> DeleteUser(string id);
        ///////////////////////////////////////////////////////////////////
        public Task<IEnumerable<ApplicationRole>> GetAllRoles();
        public Task<ApplicationRole> AddRole(ApplicationRole model);
        public Task<ApplicationRole> EditRole(ApplicationRole model, string id);
        public Task<ApplicationRole> GetRoleById(string id);
        public Task<ApplicationRole> DeleteRole(string id);
        /////////////////////////////////////////////////////////////////
        public Task<IEnumerable<UserRoleViewModel>> GetAllUserRole();
        public Task<UserRoleViewModel> AddUserRole(UserRoleViewModel model);
        public Task<UserRoleViewModel> EditUserRole(UserRoleViewModel model);
        public Task<UserRoleViewModel> DeleteUserRole(UserRoleViewModel model);
        /////////////////////////////////////////////////////////////////////////
        public Task<IEnumerable<Category>> GetAllCategories();
        public Task<Category> AddCategory(Category model);
        public Task<Category> EditCategory(Category model, int id);
        public Task<Category> GetCategoryById(int id);
        public Task<Category> DeleteCategory(int id);
        ////////////////////////////////////////////////////////////////////////////////
        public Task<IEnumerable<Product>> GetAllProducts();
        public Task<Product> AddProduct(ProductViewModel model);
        public Task<Product> EditProduct(ProductViewModel model, int id);
        public Task<Product> GetProductById(int id);
        public Task<Product> DeleteProduct(int id);
        ////////////////////////////////////////////////////////////////////////////////
        public Task<IEnumerable<Order>> GetAllOrders();
        public Task<Order> AddOrder(OrderViewModel model);
        public Task<Order> EditOrder(OrderViewModel model, int id);
        public Task<Order> GetOrderById(int id);
        public Task<Order> DeleteOrder(int id);
    }
}
