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

        public async Task<User> FindUser(string username)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User> FindUserByUsername(string username)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == username);
            if (user is null)
                throw new Exception("User not found");
            return user;
        }

        public async Task<User> FindUsernameDb(string username)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Username == username);
        }

        // Mengambil all user dengan role user
        public async Task<IEnumerable<User>> GetAllUser()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUser(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(o => o.Username == username && o.Deletes == false);
            // jika user tidak ditemukan
            if (user is null)
                throw new ArgumentException($"Username '{username}' not found or is banned.");
            return user;
        }

        public async Task UpdateUser(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
