using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class ServiceDAO
    {
        private static ServiceDAO instance = null;
        private static readonly object instanceLock = new object();
        private ServiceDAO() { }
        public static ServiceDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new ServiceDAO();
                    }
                    return instance;
                }
            }
        }
        public static async Task<List<Service>> GetServices()
        {
            var services = new List<Service>();

            try
            {
                using (var context = new CinemaManagementContext())
                {
                    services = await context.Services.ToListAsync();

                }
                return services;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        public static async Task AddService(Service m)
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
                            context.Services.Add(m);
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

        public static async Task UpdatService(Service m)
        {
            try
            {
                using (var context = new CinemaManagementContext())
                {


                    context.Entry<Service>(m).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    await context.SaveChangesAsync();

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task DeleteService(int p)
        {
            try
            {
                using (var context = new CinemaManagementContext())
                {
                    var service = await context.Services.FirstOrDefaultAsync(c => c.Id == p);
                    if (service == null)
                    {
                        throw new Exception("Id is not Exits");
                    }
                    else
                    {
                        context.Services.Remove(service);
                        await context.SaveChangesAsync();
                    }



                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public async Task<List<Service>> SearchByTitle(string search, int page, int pageSize)
        {
            List<Service> searchResult = null;
            if (page == 0 || pageSize == 0)
            {
                page = 1;
                pageSize = 1000;
            }
            if (search == null)
            {
                IEnumerable<Service> searchValues = await GetServices();
                searchValues = searchValues.Skip((page - 1) * pageSize).Take(pageSize);
                searchResult = searchValues.ToList();
            }
            else
            {
                using (var context = new CinemaManagementContext())
                {
                    IEnumerable<Service> searchValues = await (from service in context.Services
                                                        where service.Title.ToLower().Contains(search.ToLower())
                                                      select service).ToListAsync();
                    searchValues = searchValues.Skip((page - 1) * pageSize).Take(pageSize);
                    searchResult = searchValues.ToList();
                }
            }

            return searchResult;
        }
        public async Task<Service> GetServiceById(int TypeId)
        {

            IEnumerable<Service> services = await GetServices();
            Service service = services.SingleOrDefault(mb => mb.Id == TypeId);
            return service;
        }
        public async Task UpdateActive(int ServiceId, bool? acticve)
        {

            try
            {

                var service = new Service() { Id = ServiceId, Active = !acticve };
                using (var db = new CinemaManagementContext())
                {
                    db.Services.Attach(service);
                    db.Entry(service).Property(x => x.Active).IsModified = true;
                    await db.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task UpdateQuantity(int ServiceId, int quantity)
        {

            try
            {

                var service = new Service() { Id = ServiceId, Quantity = quantity };
                using (var db = new CinemaManagementContext())
                {
                    db.Services.Attach(service);
                    db.Entry(service).Property(x => x.Quantity).IsModified = true;
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
