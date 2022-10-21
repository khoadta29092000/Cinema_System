using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class CinemaDAO
    {
        private static CinemaDAO instance = null;
        private static readonly object instanceLock = new object();
        private CinemaDAO() { }
        public static CinemaDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new CinemaDAO();
                    }
                    return instance;
                }
            }
        }
        public static async Task<List<Cinema>> GetCinemas()
        {
            var cinemas = new List<Cinema>();

            try
            {
                using (var context = new CinemaManagementContext())
                {
                    cinemas = await context.Cinemas.ToListAsync();

                }
                return cinemas;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

            public static async Task<List<Location>> GetLocaitons()
            {
                var locations = new List<Location>();

                try
                {
                    using (var context = new CinemaManagementContext())
                    {
                    locations = await context.Locations.ToListAsync();

                    }
                    return locations;
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }

            }

            public static async Task AddCinema(Cinema m)
        {
            try
            {


                using (var context = new CinemaManagementContext())
                {
                    var p1 = await context.Types.FirstOrDefaultAsync(c => c.Title.Equals(m.Name));
                    var p2 = await context.Types.FirstOrDefaultAsync(c => c.Id.Equals(m.Id));
                    if (p1 == null)
                    {
                        if (p2 == null)
                        {
                            context.Cinemas.Add(m);
                            await context.SaveChangesAsync();
                        }
                        else
                        {
                            throw new Exception("Id is Exits");
                        }
                    }
                    else
                    {
                        throw new Exception("Name is Exist");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task UpdateCinema(Cinema m)
        {
            try
            {
                using (var context = new CinemaManagementContext())
                {


                    context.Entry<Cinema>(m).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    await context.SaveChangesAsync();

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task DeleteCinema(int p)
        {
            try
            {
                using (var context = new CinemaManagementContext())
                {
                    var cinema = await context.Cinemas.FirstOrDefaultAsync(c => c.Id == p);
                    if (cinema == null)
                    {
                        throw new Exception("Id is not Exits");
                    }
                    else
                    {
                        context.Cinemas.Remove(cinema);
                        await context.SaveChangesAsync();
                    }



                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public async Task<List<Cinema>> SearchByName(string search, int page, int pageSize)
        {
            List<Cinema> searchResult = null;
            if (page == 0 || pageSize == 0)
            {
                page = 1;
                pageSize = 1000;
            }
            if (search == null)
            {
                IEnumerable<Cinema> searchValues = await GetCinemas();
                searchValues = searchValues.Skip((page - 1) * pageSize).Take(pageSize);
                searchResult = searchValues.ToList();
            }
            else
            {
                using (var context = new CinemaManagementContext())
                {
                    IEnumerable<Cinema> searchValues = await (from cinema in context.Cinemas
                                                       where cinema.Name.ToLower().Contains(search.ToLower())
                                                      select cinema).ToListAsync();
                    searchValues = searchValues.Skip((page - 1) * pageSize).Take(pageSize);
                    searchResult = searchValues.ToList();
                }
            }

            return searchResult;
        }
        public async Task<Cinema> GetCinemaById(int CinemaId)
        {

            IEnumerable<Cinema> cinemas = await GetCinemas();
            Cinema cinema = cinemas.SingleOrDefault(mb => mb.Id == CinemaId);
            return cinema;
        }
        public async Task UpdateActive(int CinemaId, bool? acticve)
        {

            try
            {

                var cinema = new Cinema() { Id = CinemaId, Active = !acticve };
                using (var db = new CinemaManagementContext())
                {
                    db.Cinemas.Attach(cinema);
                    db.Entry(cinema).Property(x => x.Active).IsModified = true;
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
