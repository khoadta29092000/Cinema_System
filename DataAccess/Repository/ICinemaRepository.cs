using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface ICinemaRepository
    {
        Task<List<Cinema>> GetCinemas();
        Task<List<Location>> GetLocations();
        Task<Cinema> GetCinemaById(int CinemaId);
        Task UpdateActive(int CinemaId, bool? active);
        Task DeleteCinema(int m);

        Task UpdateCinema(Cinema m);
        Task AddCinema(Cinema m);
        Task<List<Cinema>> SearchByName(string search, int page, int pageSize);
    }
}
