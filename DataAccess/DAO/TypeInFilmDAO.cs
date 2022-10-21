using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
namespace DataAccess.DAO
{
    public class TypeInFilmDAO
    {
        private static TypeInFilmDAO instance = null;
        private static readonly object instanceLock = new object();
        private TypeInFilmDAO() { }
        public static TypeInFilmDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new TypeInFilmDAO();
                    }
                    return instance;
                }
            }
        }
        public static async Task<List<TypeInFilm>> GetTypeInFilms()
        {
            var typeInFilms = new List<TypeInFilm>();

            try
            {
                using (var context = new CinemaManagementContext())
                {
                    typeInFilms = await context.TypeInFilms.ToListAsync();

                }
                return typeInFilms;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        public static async Task AddTypeInFilm(TypeInFilm m)
        {
            try
            {
                using (var context = new CinemaManagementContext())
                {
                    var p1 = await context.TypeInFilms.FirstOrDefaultAsync(c => c.FilmId.Equals(m.FilmId) && c.TypeId.Equals(m.TypeId));      
                    if (p1 == null)
                    {                    
                            context.TypeInFilms.Add(m);
                            await context.SaveChangesAsync();                    
                    }
                    else
                    {
                        throw new Exception("Type In Film is Exist");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task UpdateTypeInFilm(TypeInFilm m)
        {
            try
            {
                using (var context = new CinemaManagementContext())
                {


                    context.Entry<TypeInFilm>(m).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    await context.SaveChangesAsync();

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task DeleteTypeInFilm(int TypeId, int FilmId)
        {
            try
            {
                using (var context = new CinemaManagementContext())
                {
                    var type = await context.TypeInFilms.FirstOrDefaultAsync(c => c.FilmId == FilmId && c.TypeId == TypeId);
                    if (type == null)
                    {
                        throw new Exception("Id is not Exits");
                    }
                    else
                    {
                        context.TypeInFilms.Remove(type);
                        await context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<TypeInFilm> GetTypeInFilmById(int TypeId, int FilmId)
        {

            IEnumerable<TypeInFilm> typeInFilms = await GetTypeInFilms();
            TypeInFilm typeInFilm = typeInFilms.SingleOrDefault(mb => mb.TypeId == TypeId && mb.FilmId == FilmId);
            return typeInFilm;
        }
        public static async Task<List<Film>> GetTypeInFilm(int FilmId)
        {
            var searchResult = new List<Film>();
            List<Film> searchResult1 = null;

            // IEnumerable<MemberObject> searchResult = null;
            try
            {
                using (var context = new CinemaManagementContext())
                {

                    var searchValues =  from film in context.Films
                                       where film.TypeInFilms.Any(c => c.FilmId == FilmId)
                                       select film;

                    searchResult1 = await searchValues.ToListAsync();
                }

                return searchResult1;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        public async Task<List<TypeInFilm>> SearchByTypeId(int FilmId, int page, int pageSize)
        {
            List<TypeInFilm> searchResult = null;
            if (page == 0 || pageSize == 0)
            {
                page = 1;
                pageSize = 1000;
            }
            if (FilmId == 0)
            {
                IEnumerable<TypeInFilm> searchValues = await GetTypeInFilms();
                searchValues = searchValues.Skip((page - 1) * pageSize).Take(pageSize);
                searchResult = searchValues.ToList();
            }
            else
            {
                using (var context = new CinemaManagementContext())
                {
                    IEnumerable<TypeInFilm> searchValues = await (from type in context.TypeInFilms
                                                      where type.FilmId.Equals(FilmId)
                                                      select type).ToListAsync();
                    searchValues = searchValues.Skip((page - 1) * pageSize).Take(pageSize);
                    searchResult = searchValues.ToList();
                }
            }

            return searchResult;
        }
        public async Task UpdateActive(int TypeId, int FilmId, bool? acticve)
        {

            try
            {

                var typeInFilm = new TypeInFilm() { TypeId = TypeId, FilmId = FilmId, Active = !acticve };
                using (var db = new CinemaManagementContext())
                {
                    db.TypeInFilms.Attach(typeInFilm);
                    db.Entry(typeInFilm).Property(x => x.Active).IsModified = true;
                    await db.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
