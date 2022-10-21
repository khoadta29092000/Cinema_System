using BusinessObject.Models;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class TypeInFilmRepository : ITypeInFilmRepository
    {
        public Task<List<TypeInFilm>> GetTypeInFilms() => TypeInFilmDAO.GetTypeInFilms();
        public Task AddTypeInFilm(TypeInFilm m) => TypeInFilmDAO.AddTypeInFilm(m);
        public Task UpdateTypeInFilm(TypeInFilm m) => TypeInFilmDAO.UpdateTypeInFilm(m);
        public Task DeleteTypeInFilm(int TypeId, int FilmId) => TypeInFilmDAO.DeleteTypeInFilm(TypeId,FilmId);
        public Task<TypeInFilm> GetTypeInFilmById(int TypeId, int FilmId) => TypeInFilmDAO.Instance.GetTypeInFilmById(TypeId, FilmId);
        public Task<List<Film>> GetTypeInFilm(int FilmId) => TypeInFilmDAO.GetTypeInFilm(FilmId);
        public Task<List<TypeInFilm>> SearchByTypeId(int TypeId, int page, int pageSize) => TypeInFilmDAO.Instance.SearchByTypeId(TypeId, page, pageSize);
        public Task UpdateActive(int TypeId, int FilmId, bool? acticve) => TypeInFilmDAO.Instance.UpdateActive(TypeId, FilmId, acticve);
    }
}
