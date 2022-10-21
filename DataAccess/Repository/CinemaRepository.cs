using BusinessObject.Models;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class CinemaRepository : ICinemaRepository
    {
        public Task<List<Cinema>> GetCinemas() => CinemaDAO.GetCinemas();
        public Task<List<Location>> GetLocations() => CinemaDAO.GetLocaitons();
        public Task<Cinema> GetCinemaById(int CinemaId) => CinemaDAO.Instance.GetCinemaById(CinemaId);
        public Task DeleteCinema(int m) => CinemaDAO.DeleteCinema(m);

        public Task UpdateActive(int CinemaId, bool? active) => CinemaDAO.Instance.UpdateActive(CinemaId, active);
        public Task AddCinema(Cinema m) => CinemaDAO.AddCinema(m);
        public Task UpdateCinema(Cinema m) => CinemaDAO.UpdateCinema(m);
        public Task<List<Cinema>> SearchByName(string search, int page, int pageSize) => CinemaDAO.Instance.SearchByName(search, page, pageSize);
    }
}
