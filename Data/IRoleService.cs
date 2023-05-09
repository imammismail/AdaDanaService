using AdaDanaApi.Models;

namespace AdaDanaApi.Data
{
    public interface IRoleService
    {
        Role GetRoleByName(string name);
    }
}
