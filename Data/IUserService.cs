using AdaDanaApi.Models;

namespace AdaDanaApi.Data
{
    public interface IUserService
    {
        User GetUserByUsername(string username);
        void AddUser(User user);
        IEnumerable<User> GetAllUser();
    }
}
