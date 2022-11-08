using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;


namespace DataAccess.DAO
{
    public class ServiceInCinemaDAO
    {
        private static ServiceInCinemaDAO instance = null;
        private static readonly object instanceLock = new object();
        private ServiceInCinemaDAO() { }
        public static ServiceInCinemaDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new ServiceInCinemaDAO();
                    }
                    return instance;
                }
            }
        }
        public static async Task<List<ServiceInCinema>> GetServiceInCinemas()
        {
            var  serviceInCinemas = new List<ServiceInCinema>();

            try
            {
                using (var context = new CinemaManagementContext())
                {
                    serviceInCinemas = await context.ServiceInCinemas.ToListAsync();

                }
                return serviceInCinemas;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        public static async Task AddServiceInCinema(ServiceInCinema m)
        {
            try
            {
                using (var context = new CinemaManagementContext())
                {
                    var p1 = await context.ServiceInCinemas.FirstOrDefaultAsync(c => c.ServiceId.Equals(m.ServiceId) && c.CinemaId.Equals(m.CinemaId));
                    if (p1 == null)
                    {
                        context.ServiceInCinemas.Add(m);
                        await context.SaveChangesAsync();
                    }
                    else
                    {
                        throw new Exception("Service In Cinema is Exist");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task UpdateServiceInCinema(ServiceInCinema m)
        {
            try
            {
                using (var context = new CinemaManagementContext())
                {


                    context.Entry<ServiceInCinema>(m).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    await context.SaveChangesAsync();

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task DeleteServiceInCinema(int Id)
        {
            try
            {
                using (var context = new CinemaManagementContext())
                {
                    var film = await context.ServiceInCinemas.FirstOrDefaultAsync(c => c.Id == Id );
                    if (film == null)
                    {
                        throw new Exception("Id is not Exits");
                    }
                    else
                    {
                        context.ServiceInCinemas.Remove(film);
                        await context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<ServiceInCinema> GetServiceInCinemaById(int Id)
        {

            IEnumerable<ServiceInCinema> serviceInCinemas = await GetServiceInCinemas();
            ServiceInCinema serviceInCinema = serviceInCinemas.SingleOrDefault(mb => mb.Id == Id);
            return serviceInCinema;
        }

        public async Task<List<ServiceInCinema>> SearchByCinemaId(int CinemaId, int page, int pageSize)
        {
            List<ServiceInCinema> searchResult = null;
            if (page == 0 || pageSize == 0)
            {
                page = 1;
                pageSize = 1000;
            }if(CinemaId == 0)
            {
                IEnumerable<ServiceInCinema> searchValues = await GetServiceInCinemas();
                searchValues = searchValues.Skip((page - 1) * pageSize).Take(pageSize);
                searchResult = searchValues.ToList();
            }
            else
            {
                using (var context = new CinemaManagementContext())
                {
                    IEnumerable<ServiceInCinema> searchValues = await (from type in context.ServiceInCinemas
                                                                       where type.CinemaId.Equals(CinemaId)
                                                                       select type).ToListAsync();
                    searchValues = searchValues.Skip((page - 1) * pageSize).Take(pageSize);
                    searchResult = searchValues.ToList();
                }
            }
          

            return searchResult;
        }
        public async Task<List<ServiceInCinema>> SearchByServiceId(int ServiceId, int page, int pageSize)
        {
            List<ServiceInCinema> searchResult = null;
            if (page == 0 || pageSize == 0)
            {
                page = 1;
                pageSize = 1000;
            }
            using (var context = new CinemaManagementContext())
            {
                IEnumerable<ServiceInCinema> searchValues = await (from type in context.ServiceInCinemas
                                                                   where type.ServiceId.Equals(ServiceId)
                                                                select type).ToListAsync();
                searchValues = searchValues.Skip((page - 1) * pageSize).Take(pageSize);
                searchResult = searchValues.ToList();
            }
            return searchResult;
        }
        public static async Task<List<ServiceInCinemaDTO>> GetAllServiceInCinema(int CinemaId, int page, int pageSize)
        {
            var searchResult = new List<ServiceInCinemaDTO>();
            List<ServiceInCinemaDTO> searchResult1 = null;

            // IEnumerable<MemberObject> searchResult = null;
            try
            {
                if (page == 0 || pageSize == 0)
                {
                    page = 1;
                    pageSize = 1000;
                }
                    using (var context = new CinemaManagementContext())
                    {
                  
                    IEnumerable<ServiceInCinemaDTO> searchValues = await (from serviceInCinema in context.ServiceInCinemas
                                                                          join service in context.Services on serviceInCinema.ServiceId equals service.Id
                                                                          where serviceInCinema.CinemaId == CinemaId
                                                                          select  new ServiceInCinemaDTO
                                                               {
                                                                   Active = service.Active,
                                                                   Description = service.Description,
                                                                   Id = service.Id,
                                                                   Title = service.Title,
                                                                   Price = service.Price,
                                                                   Image = service.Image,
                                                                   QuantityInCinema = serviceInCinema.Quantity,
                                                                   Quantity = service.Quantity
                                                               }).ToListAsync();
                    searchValues = searchValues.Skip((page - 1) * pageSize).Take(pageSize);
                        searchResult1 = searchValues.ToList();
                        // searchValues =  searchValues.Skip((page - 1) * pageSize).Take(pageSize);

                    }

                
               
                return searchResult1;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }
        public static async Task<List<Cinema>> GetAllCinemaHaveService(int ServiceId, int page, int pageSize)
        {
            var searchResult = new List<Cinema>();
            List<Cinema> searchResult1 = null;

            // IEnumerable<MemberObject> searchResult = null;
            try
            {
                if (page == 0 || pageSize == 0)
                {
                    page = 1;
                    pageSize = 1000;
                }
                using (var context = new CinemaManagementContext())
                {

                    IEnumerable<Cinema> searchValues = await (from film in context.Cinemas
                                                              where film.ServiceInCinemas.Any(c => c.ServiceId == ServiceId)
                                                              select film).ToListAsync();
                    searchValues = searchValues.Skip((page - 1) * pageSize).Take(pageSize);
                    searchResult1 = searchValues.ToList();
                }

                return searchResult1;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }
        public async Task UpdateActive(int Id, bool? acticve)
        {

            try
            {

                var serviceInCinema = new ServiceInCinema() { Id = Id, Active = !acticve };
                using (var db = new CinemaManagementContext())
                {
                    db.ServiceInCinemas.Attach(serviceInCinema);
                    db.Entry(serviceInCinema).Property(x => x.Active).IsModified = true;
                    await db.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task UpdateQuantity(int Id, int quantity)
        {

            try
            {

                var serviceInCinema = new ServiceInCinema() { Id = Id, Quantity = quantity };
                using (var db = new CinemaManagementContext())
                {
                    db.ServiceInCinemas.Attach(serviceInCinema);
                    db.Entry(serviceInCinema).Property(x => x.Quantity).IsModified = true;
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
