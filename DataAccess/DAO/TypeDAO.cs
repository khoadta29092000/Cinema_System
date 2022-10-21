using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DAO
{
    public class TypeDAO
    {
        private static TypeDAO instance = null;
        private static readonly object instanceLock = new object();
        private TypeDAO() { }
        public static TypeDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new TypeDAO();
                    }
                    return instance;
                }
            }
        }
        public static async Task<List<Types>> GetTypes()
        {
            var types = new List<Types>();

            try
            {
                using (var context = new CinemaManagementContext())
                {
                    types = await context.Types.ToListAsync();

                }
                return types;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }
  
        public static async Task AddType(Types m)
        {
            try
            {


                using (var context = new CinemaManagementContext())
                {
                    var p1 = await context.Types.FirstOrDefaultAsync(c => c.Title.Equals(m.Title));
                    var p2 = await context.Types.FirstOrDefaultAsync(c => c.Id.Equals(m.Id));
                    if (p1 == null)
                    {
                        if (p2 == null)
                        {
                            context.Types.Add(m);
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

        public static async Task UpdateType(Types m)
        {
            try
            {
                using (var context = new CinemaManagementContext())
                {


                    context.Entry<Types>(m).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    await context.SaveChangesAsync();

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task DeleteType(int p)
        {
            try
            {
                using (var context = new CinemaManagementContext())
                {
                    var type = await context.Types.FirstOrDefaultAsync(c => c.Id == p);
                    if (type == null)
                    {
                        throw new Exception("Id is not Exits");
                    }
                    else
                    {
                        context.Types.Remove(type);
                        await context.SaveChangesAsync();
                    }



                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public async Task<List<Types>> SearchByTitle(string search, int page, int pageSize)
        {
            List<Types> searchResult = null;
            if (page == 0 || pageSize == 0)
            {
                page = 1;
                pageSize = 1000;
            }
            if (search == null)
            {
                IEnumerable<Types> searchValues = await GetTypes();
                searchValues = searchValues.Skip((page - 1) * pageSize).Take(pageSize);
                searchResult = searchValues.ToList();
            }
            else
            {
                using (var context = new CinemaManagementContext())
                {
                    IEnumerable<Types> searchValues = await (from type in context.Types
                                       where type.Title.ToLower().Contains(search.ToLower())
                                       select type).ToListAsync();
                    searchValues = searchValues.Skip((page - 1) * pageSize).Take(pageSize);
                    searchResult = searchValues.ToList();
                }
            }

            return searchResult;
        }
        public async Task<Types> GetTypeById(int TypeId)
        {

            IEnumerable<Types> types = await GetTypes();
            Types type = types.SingleOrDefault(mb => mb.Id == TypeId);
            return type;
        }
        public async Task UpdateActive(int TypeId, bool? acticve)
        {

            try
            {

                var type = new Types() { Id = TypeId, Active = !acticve };
                using (var db = new CinemaManagementContext())
                {
                    db.Types.Attach(type);
                    db.Entry(type).Property(x => x.Active).IsModified = true;
                    await db.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }

}
