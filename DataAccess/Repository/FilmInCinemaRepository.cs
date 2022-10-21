using BusinessObject.Models;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class FilmInCinemaRepository : IFilmInCinemaRepository
    {
        public Task<List<FilmInCinema>> GetFilmInCinemas() => FilmInCinemaDAO.GetFilmInCinemas();
        public Task AddFilmInCinema(FilmInCinema m) => FilmInCinemaDAO.AddFilmInCinema(m);

        public Task UpdateFilmInCinema(FilmInCinema m) => FilmInCinemaDAO.UpdateFilmInCinema(m);
        public Task DeleteFilmInCinema(int CinemaId, int FilmId) => FilmInCinemaDAO.DeleteFilmInCinema(CinemaId, FilmId);
        public Task<FilmInCinema> GetFilmInCinemaById(int CinemaId, int FilmId) => FilmInCinemaDAO.Instance.GetFilmInCinemaById(CinemaId, FilmId);
        public Task<List<FilmInCinema>> SearchByCinemaId(int CinemaId, int page, int pageSize) => FilmInCinemaDAO.Instance.SearchByCinemaId(CinemaId, page, pageSize);
        public Task<List<FilmInCinema>> SearchByFilmId(int FilmId, int page, int pageSize) => FilmInCinemaDAO.Instance.SearchByFilmId(FilmId, page, pageSize);
        public Task<List<Film>> GetAllFilmInCinema(int CinemaId, int page, int pageSize) => FilmInCinemaDAO.GetAllFilmInCinema(CinemaId, page, pageSize);
        public Task<List<Cinema>> GetAllCinemaHaveFilm(int FilmId, int page, int pageSize) => FilmInCinemaDAO.GetAllCinemaHaveFilm(FilmId, page, pageSize);
    }
}
