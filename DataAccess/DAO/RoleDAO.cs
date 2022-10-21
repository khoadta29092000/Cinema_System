using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DAO
{
    public class RoleDAO
    {
        private static RoleDAO instance = null;
        private static readonly object instanceLock = new object();
        private RoleDAO() { }
        public static RoleDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new RoleDAO();
                    }
                    return instance;
                }
            }
        }
        public static async Task<List<Role>> GetRoles()
        {
            var roles = new List<Role>();

            try
            {
                using (var context = new CinemaManagementContext())
                {
                    roles = await context.Roles.ToListAsync();

                }
                return roles;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        public static async Task AddRole(Role m)
        {
            try
            {


                using (var context = new CinemaManagementContext())
                {
                    var p1 = await context.Roles.FirstOrDefaultAsync(c => c.Title.Equals(m.Title));
                    var p2 = await context.Roles.FirstOrDefaultAsync(c => c.Id.Equals(m.Id));
                    if (p1 == null)
                    {
                        if (p2 == null)
                        {
                            context.Roles.Add(m);
                            await context.SaveChangesAsync();
                        }
                        else
                        {
                            throw new Exception("Id is Exits");
                        }
                    }
                    else
                    {
                        throw new Exception("Title is Exist");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task UpdateRole(Role m)
        {
            try
            {
                using (var context = new CinemaManagementContext())
                {


                    context.Entry<Role>(m).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    await context.SaveChangesAsync();

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task DeleteRole(int p)
        {
            try
            {
                using (var context = new CinemaManagementContext())
                {
                    var role = await context.Roles.FirstOrDefaultAsync(c => c.Id == p);
                    if (role == null)
                    {
                        throw new Exception("Id is not Exits");
                    }
                    else
                    {
                        context.Roles.Remove(role);
                        await context.SaveChangesAsync();
                    }



                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
      
        public async Task<Role> GetRoleById(int Id)
        {

            IEnumerable<Role> roles = await GetRoles();
            Role role = roles.SingleOrDefault(mb => mb.Id == Id);
            return role;
        }
       
    }
}
