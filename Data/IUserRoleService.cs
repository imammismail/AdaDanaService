using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdaDanaService.Data
{
    public interface IUserRoleService
    {
        List<string> GetRolesByUsername(string username);
    }
}