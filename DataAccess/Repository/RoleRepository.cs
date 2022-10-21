using BusinessObject.Models;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class RoleRepository : IRoleRepository
    {
        public Task<List<Role>> GetRoles() => RoleDAO.GetRoles();
        public Task<Role> GetRoleById(int Id) => RoleDAO.Instance.GetRoleById(Id);
        public Task DeleteRole(int m) => RoleDAO.DeleteRole(m);  
        public Task AddRole(Role m) => RoleDAO.AddRole(m);
        public Task UpdateRole(Role m) => RoleDAO.UpdateRole(m);
    }
}
