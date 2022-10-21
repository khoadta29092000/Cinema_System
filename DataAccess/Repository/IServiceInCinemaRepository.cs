using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IServiceInCinemaRepository
    {
        Task<List<ServiceInCinema>> GetServicenCinemas();
        Task AddServiceInCinema(ServiceInCinema m);

        Task UpdateServiceInCinema(ServiceInCinema m);
        Task DeleteServiceInCinema(int Id);
        Task UpdateActive(int Id, bool? active);
        Task UpdateQuantity(int Id, int Quantity);
        Task<ServiceInCinema> GetServiceInCinemaById(int Id);
        Task<List<ServiceInCinema>> SearchByCinemaId(int CinemaId, int page, int pageSize);
        Task<List<ServiceInCinema>> SearchByServiceId(int ServiceId, int page, int pageSize);
        Task<List<Service>> GetAllServiceInCinema(int CinemaId, int page, int pageSize);
        Task<List<Cinema>> GetAllCinemaHaveService(int ServiceId, int page, int pageSize);
    }
}
