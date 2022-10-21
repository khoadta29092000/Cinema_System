using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IFilmRepository
    {
        Task<List<Film>> GetFilms();

        Task<Film> GetFilmById(int FilmId);
        Task UpdateActive(int FilmId, bool? active);
        Task DeleteFilm(int m);

        Task UpdateFilm(Film m);
        Task AddFilm(Film m);
        Task<List<Film>> SearchByTitle(string search, int page, int pageSize);
    }
}
