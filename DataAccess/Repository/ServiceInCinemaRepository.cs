using BusinessObject.Models;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class ServiceInCinemaRepository : IServiceInCinemaRepository
    {
        public Task<List<ServiceInCinema>> GetServicenCinemas() => ServiceInCinemaDAO.GetServiceInCinemas();
        public Task AddServiceInCinema(ServiceInCinema m) => ServiceInCinemaDAO.AddServiceInCinema(m);

        public Task UpdateServiceInCinema(ServiceInCinema m) => ServiceInCinemaDAO.UpdateServiceInCinema(m);
        public Task DeleteServiceInCinema(int Id) => ServiceInCinemaDAO.DeleteServiceInCinema(Id);
        public Task UpdateActive(int Id, bool? active) => ServiceInCinemaDAO.Instance.UpdateActive(Id, active);
        public Task UpdateQuantity(int Id, int Quantity) => ServiceInCinemaDAO.Instance.UpdateQuantity(Id, Quantity);
        public Task UpdateQuantityInCinema(int serviceId,int cinemaId ,int Quantity) => ServiceInCinemaDAO.Instance.UpdateQuantityInCinema(serviceId, cinemaId, Quantity); 
        public Task<ServiceInCinema> GetServiceInCinemaById(int Id) => ServiceInCinemaDAO.Instance.GetServiceInCinemaById(Id);
        public Task<ServiceInCinema> GetServiceById(int ServiceId, int cinemaId) => ServiceInCinemaDAO.Instance.GetServiceById(ServiceId, cinemaId);
        public Task<List<ServiceInCinema>> SearchByCinemaId(int CinemaId, int page, int pageSize) => ServiceInCinemaDAO.Instance.SearchByCinemaId(CinemaId, page, pageSize);
        public Task<List<ServiceInCinema>> SearchByServiceId(int ServiceId, int page, int pageSize) => ServiceInCinemaDAO.Instance.SearchByServiceId(ServiceId, page, pageSize);
        public Task<List<ServiceInCinemaDTO>> GetAllServiceInCinema(int CinemaId, int page, int pageSize) => ServiceInCinemaDAO.GetAllServiceInCinema(CinemaId, page, pageSize);
        public Task<List<Cinema>> GetAllCinemaHaveService(int ServiceId, int page, int pageSize) => ServiceInCinemaDAO.GetAllCinemaHaveService(ServiceId, page, pageSize);
    }
}
