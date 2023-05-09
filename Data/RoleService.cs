using AdaDanaService.Models;

namespace AdaDanaService.Data
{
    public class RoleService : IRoleService
    {
        private readonly AdaDanaContext _context;

        public RoleService(AdaDanaContext context)
        {
            _context = context;
        }
        public Role GetRoleByName(string name)
        {
            var nameRole = _context.Roles.FirstOrDefault(r => r.Name == name);
            if (nameRole is null)
                throw new Exception("Role user not found");
            return nameRole;
        }
    }
}
