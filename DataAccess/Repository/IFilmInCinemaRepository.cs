using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IFilmInCinemaRepository
    {
         Task<List<FilmInCinema>> GetFilmInCinemas();
         Task AddFilmInCinema(FilmInCinema m);

         Task UpdateFilmInCinema(FilmInCinema m);
         Task DeleteFilmInCinema(int CinemaId, int FilmId);
         Task<FilmInCinema> GetFilmInCinemaById(int CinemaId, int FilmId);
         Task<List<FilmInCinema>> SearchByCinemaId(int CinemaId, int page, int pageSize);
         Task<List<FilmInCinema>> SearchByFilmId(int FilmId, int page, int pageSize);
         Task<List<Film>> GetAllFilmInCinema(int CinemaId, int page, int pageSize);
         Task<List<Cinema>> GetAllCinemaHaveFilm(int FilmId, int page, int pageSize);

    }
}
