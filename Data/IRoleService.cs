using AdaDanaService.Models;

namespace AdaDanaService.Data
{
    public interface IRoleService
    {
        Role GetRoleByName(string name);
    }
}
