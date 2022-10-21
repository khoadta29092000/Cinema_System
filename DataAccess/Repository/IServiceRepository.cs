using BusinessObject.Models;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IServiceRepository
    {
        Task<List<Service>> GetServices();

        Task<Service> GetServiceById(int ServiceId);
        Task UpdateActive(int ServiceId, bool? active);
        Task UpdateQuantity(int ServiceId, int quantity);
        Task DeleteService(int m);

        Task UpdateService(Service m);
        Task AddService(Service m);
        Task<List<Service>> SearchByTitle(string search, int page, int pageSize);
    }
}
