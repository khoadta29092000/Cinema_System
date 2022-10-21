using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;


namespace DataAccess.DAO
{
    public class ServiceInBillDAO
    {
        private static ServiceInBillDAO instance = null;
        private static readonly object instanceLock = new object();
        private ServiceInBillDAO() { }
        public static ServiceInBillDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new ServiceInBillDAO();
                    }
                    return instance;
                }
            }
        }
        public static async Task<List<ServiceInBill>> GetServiceInBills()
        {
            var serviceInBills = new List<ServiceInBill>();

            try
            {
                using (var context = new CinemaManagementContext())
                {
                    serviceInBills = await context.ServiceInBills.ToListAsync();

                }
                return serviceInBills;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        public static async Task AddServiceInBill(ServiceInBill m)
        {
            try
            {
                using (var context = new CinemaManagementContext())
                {
                    var p1 = await context.ServiceInBills.FirstOrDefaultAsync(c => c.ServiceInCinemaId.Equals(m.ServiceInCinemaId) && c.BillId.Equals(m.BillId));
                    if (p1 == null)
                    {
                        context.ServiceInBills.Add(m);
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

        public static async Task UpdateServiceInBill(ServiceInBill m)
        {
            try
            {
                using (var context = new CinemaManagementContext())
                {


                    context.Entry<ServiceInBill>(m).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    await context.SaveChangesAsync();

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task DeleteServiceInBill(int ServiceInCinemaId, int BillId)
        {
            try
            {
                using (var context = new CinemaManagementContext())
                {
                    var serviceInBill = await context.ServiceInBills.FirstOrDefaultAsync(c => c.ServiceInCinemaId == ServiceInCinemaId && c.BillId == BillId);
                    if (serviceInBill == null)
                    {
                        throw new Exception("Id is not Exits");
                    }
                    else
                    {
                        context.ServiceInBills.Remove(serviceInBill);
                        await context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<ServiceInBill> GetServiceInBillById(int ServiceInCinemaId, int BillId)
        {

            IEnumerable<ServiceInBill> serviceInCinemas = await GetServiceInBills();
            ServiceInBill serviceInCinema = serviceInCinemas.SingleOrDefault(mb => mb.ServiceInCinemaId == ServiceInCinemaId && mb.BillId == BillId);
            return serviceInCinema;
        }

        public async Task<List<ServiceInBill>> SearchByCinemaId(int ServiceId, int page, int pageSize)
        {
            List<ServiceInBill> searchResult = null;
            if (page == 0 || pageSize == 0)
            {
                page = 1;
                pageSize = 1000;
            }
            if (ServiceId == 0)
            {
                IEnumerable<ServiceInBill> searchValues = await GetServiceInBills();
                searchValues = searchValues.Skip((page - 1) * pageSize).Take(pageSize);
                searchResult = searchValues.ToList();
            }
            else
            {
                using (var context = new CinemaManagementContext())
                {
                  
                    IEnumerable<ServiceInBill> searchValues = await (from service in context.ServiceInBills
                                                                     where service.ServiceInCinema.ServiceId.Equals(ServiceId)
                                                                       select service).ToListAsync();
                    searchValues = searchValues.Skip((page - 1) * pageSize).Take(pageSize);
                    searchResult = searchValues.ToList();
                }
            }


            return searchResult;
        }
    
     
    }
}
