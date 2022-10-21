using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public  interface ITypeInFilmRepository
    {
        Task<List<TypeInFilm>> GetTypeInFilms();
        Task AddTypeInFilm(TypeInFilm m);
        Task UpdateTypeInFilm(TypeInFilm m);
        Task DeleteTypeInFilm(int TypeId, int FilmId);
        Task<TypeInFilm> GetTypeInFilmById(int TypeId, int FilmId);
        Task<List<Film>> GetTypeInFilm(int FilmId);
        Task<List<TypeInFilm>> SearchByTypeId(int TypeId, int page, int pageSize);

        Task UpdateActive(int TypeId, int FilmId, bool? acticve);
    }
}
