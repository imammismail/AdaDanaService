using AdaDanaService.Models;

namespace AdaDanaService.Data
{
    public interface IUserService
    {
        User GetUserByUsername(string username);
        void AddUser(User user);
        IEnumerable<User> GetAllUser();
    }
}
