using BusinessObject.Models;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class FilmRepository : IFilmRepository
    {
        public Task<List<Film>> GetFilms() => FilmDAO.GetFilms();

        public Task<Film> GetFilmById(int FilmId) => FilmDAO.Instance.GetFilmById(FilmId);
        public Task DeleteFilm(int m) => FilmDAO.DeleteFilm(m);

        public Task UpdateActive(int FilmId, bool? active) => FilmDAO.Instance.UpdateActive(FilmId, active);
        public Task AddFilm(Film m) => FilmDAO.AddFilm(m);
        public Task UpdateFilm(Film m) => FilmDAO.UpdateFilm(m);
        public Task<List<Film>> SearchByTitle(string search, int page, int pageSize) => FilmDAO.Instance.SearchByTitle(search, page, pageSize);
    }
}
