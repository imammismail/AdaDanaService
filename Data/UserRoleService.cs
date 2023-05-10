using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdaDanaService.Models;

namespace AdaDanaService.Data
{
    public class UserRoleService : IUserRoleService
    {
        private readonly AdaDanaContext _context;

        public UserRoleService(AdaDanaContext context)
        {
            _context = context;
        }

        // Tambah role ke tabel user role
        public async Task AddRoleUser(UserRole userRole)
        {
            await _context.UserRoles.AddAsync(userRole);
        }

        // public List<string> GetRolesByUsername(string username)
        //     {
        // //         return _context.Users
        // //    .Join(_context.Roles, u => u.RoleId, r => r.Id, (u, r) => new { u, r })
        // //    .Where(ur => ur.u.Username == username)
        // //    .Select(ur => ur.r.Name)
        // //    .ToList();

        //     }
    }
}