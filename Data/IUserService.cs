using AdaDanaService.Models;

namespace AdaDanaService.Data
{
    public interface IUserService
    {
        Task AddUser(User user);
        Task<User> FindUserByUsername(string username);
        Task<IEnumerable<User>> GetAllUser();
        Task<User> GetUser(string username);
        Task UpdateUser(User user);
    }
}
