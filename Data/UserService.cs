using AdaDanaService.Models;
using Microsoft.EntityFrameworkCore;

namespace AdaDanaService.Data
{
    public class UserService : IUserService
    {
        private readonly AdaDanaContext _context;

        public UserService(AdaDanaContext context)
        {
            _context = context;
        }

        // Tambah user ke tabel user
        public async Task AddUser(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task<User> FindUserByUsername(string username)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == username && u.Deletes == false);
            if (user is null)
                throw new Exception("User not found");
            return user;
        }

        // Mengambil all user dengan role user
        public async Task<IEnumerable<User>> GetAllUser()
        {
            return await _context.Users.ToListAsync();
        }
    }
}
