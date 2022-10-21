using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IRoleRepository
    {
        Task<List<Role>> GetRoles();
        Task<Role> GetRoleById(int Id);
        Task DeleteRole(int m);
        Task UpdateRole(Role m);
        Task AddRole(Role m);
        
    }
}
