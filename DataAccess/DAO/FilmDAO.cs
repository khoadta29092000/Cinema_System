using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;


namespace DataAccess.DAO
{
    public class FilmDAO
    {
        private static FilmDAO instance = null;
        private static readonly object instanceLock = new object();
        private FilmDAO() { }
        public static FilmDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new FilmDAO();
                    }
                    return instance;
                }
            }
        }
        public static async Task<List<Film>> GetFilms()
        {
            var films = new List<Film>();

            try
            {
                using (var context = new CinemaManagementContext())
                {
                    films = await context.Films.Include(x => x.TypeInFilms).ToListAsync();

                }
                return films;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        public static async Task AddFilm(Film m)
        {
            try
            {


                using (var context = new CinemaManagementContext())
                {
                    var p1 = await context.Films.FirstOrDefaultAsync(c => c.Title.Equals(m.Title));
                    var p2 = await context.Films.FirstOrDefaultAsync(c => c.Id.Equals(m.Id));
                    if (p1 == null)
                    {
                        if (p2 == null)
                        {
                            context.Films.Add(m);
                            await context.SaveChangesAsync();
                        }
                        else
                        {
                            throw new Exception("Id is Exits");
                        }
                    }
                    else
                    {
                        throw new Exception("Title is Exist");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task UpdateFilm(Film m)
        {
            try
            {
                using (var context = new CinemaManagementContext())
                {
                    var p1 = await context.Films.FirstOrDefaultAsync(c => c.Title.Equals(m.Title));

                    if (p1 == null)
                    {

                        context.Entry<Film>(m).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        await context.SaveChangesAsync();

                    }
                    else
                    {
                        if (p1.Title == m.Title)
                        {
                            context.Entry<Film>(m).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                            await context.SaveChangesAsync();
                        }

                        throw new Exception("Cinema Name is Exits");
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task DeleteFilm(int p)
        {
            try
            {
                using (var context = new CinemaManagementContext())
                {
                    var film = await context.Films.FirstOrDefaultAsync(c => c.Id == p);
                    if (film == null)
                    {
                        throw new Exception("Id is not Exits");
                    }
                    else
                    {
                        context.Films.Remove(film);
                        await context.SaveChangesAsync();
                    }



                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public async Task<List<Film>> SearchByTitle(string search, int page, int pageSize)
        {
            List<Film> searchResult = null;
            if (page == 0 || pageSize == 0)
            {
                page = 1;
                pageSize = 1000;
            }
            if (search == null)
            {
                IEnumerable<Film> searchValues = await GetFilms();
                searchValues = searchValues.Skip((page - 1) * pageSize).Take(pageSize);
                searchResult = searchValues.ToList();
            }
            else
            {
                using (var context = new CinemaManagementContext())
                {
                    IEnumerable<Film> searchValues = await (from film in context.Films
                                                      where film.Title.ToLower().Contains(search.ToLower())
                                                      select film).Include(x => x.TypeInFilms).ToListAsync();
                    searchValues = searchValues.Skip((page - 1) * pageSize).Take(pageSize);
                    searchResult = searchValues.ToList();
                }
            }

            return searchResult;
        }
        public async Task<Film> GetFilmById(int FilmId)
        {
            try
            {
                using (var context = new CinemaManagementContext())
                {
                    IEnumerable<Film> film;
                     film =  context.Films.Include(x => x.TypeInFilms).Where(x => x.Id == FilmId);
                    Film cinema = film.SingleOrDefault(mb => mb.Id == FilmId);
                    return cinema;

                }
            
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
         

        }
        public async Task UpdateActive(int FilmId, bool? acticve)
        {

            try
            {

                var film = new Film() { Id = FilmId, Active = !acticve };
                using (var db = new CinemaManagementContext())
                {
                    db.Films.Attach(film);
                    db.Entry(film).Property(x => x.Active).IsModified = true;
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
