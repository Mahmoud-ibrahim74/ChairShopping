using ChairShopping.Data;
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
    }
}
