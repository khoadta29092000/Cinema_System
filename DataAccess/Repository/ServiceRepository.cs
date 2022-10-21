using BusinessObject.Models;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class ServiceRepository : IServiceRepository
    {
        public Task<List<Service>> GetServices() => ServiceDAO.GetServices();

        public Task<Service> GetServiceById(int ServiceId) => ServiceDAO.Instance.GetServiceById(ServiceId);
        public Task DeleteService(int m) => ServiceDAO.DeleteService(m);

        public Task UpdateActive(int ServiceId, bool? active) => ServiceDAO.Instance.UpdateActive(ServiceId, active);
        public Task UpdateQuantity(int ServiceId, int quantity) => ServiceDAO.Instance.UpdateQuantity(ServiceId, quantity);
        public Task AddService(Service m) => ServiceDAO.AddService(m);
        public Task UpdateService(Service m) => ServiceDAO.UpdatService(m);
        public Task<List<Service>> SearchByTitle(string search, int page, int pageSize) => ServiceDAO.Instance.SearchByTitle(search, page, pageSize);
    }
}
