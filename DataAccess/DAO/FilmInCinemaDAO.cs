using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DAO
{
    class FilmInCinemaDAO
    {
        private static FilmInCinemaDAO instance = null;
        private static readonly object instanceLock = new object();
        private FilmInCinemaDAO() { }
        public static FilmInCinemaDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new FilmInCinemaDAO();
                    }
                    return instance;
                }
            }
        }
        public static async Task<List<FilmInCinema>> GetFilmInCinemas()
        {
            var filmInCinemas = new List<FilmInCinema>();

            try
            {
                using (var context = new CinemaManagementContext())
                {
                    filmInCinemas = await context.FilmInCinemas.ToListAsync();

                }
                return filmInCinemas;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        public static async Task AddFilmInCinema(FilmInCinema m)
        {
            try
            {
                using (var context = new CinemaManagementContext())
                {
                    var p1 = await context.FilmInCinemas.FirstOrDefaultAsync(c => c.FilmId.Equals(m.FilmId) && c.CinemaId.Equals(m.CinemaId));
                    if (p1 == null)
                    {
                        context.FilmInCinemas.Add(m);
                        await context.SaveChangesAsync();
                    }
                    else
                    {
                        throw new Exception("Film In Cinema is Exist");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task UpdateFilmInCinema(FilmInCinema m)
        {
            try
            {
                using (var context = new CinemaManagementContext())
                {


                    context.Entry<FilmInCinema>(m).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    await context.SaveChangesAsync();

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task DeleteFilmInCinema(int CinemaId, int FilmId)
        {
            try
            {
                using (var context = new CinemaManagementContext())
                {
                    var film = await context.FilmInCinemas.FirstOrDefaultAsync(c => c.FilmId == FilmId && c.CinemaId == CinemaId);
                    if (film == null)
                    {
                        throw new Exception("Id is not Exits");
                    }
                    else
                    {
                        context.FilmInCinemas.Remove(film);
                        await context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<FilmInCinema> GetFilmInCinemaById(int CinemaId, int FilmId)
        {

            IEnumerable<FilmInCinema> filmInCinemas = await GetFilmInCinemas();
            FilmInCinema filmInCinema = filmInCinemas.SingleOrDefault(mb => mb.CinemaId == CinemaId && mb.FilmId == FilmId);
            return filmInCinema;
        }

        public async Task<List<FilmInCinema>> SearchByCinemaId(int CinemaId, int page, int pageSize)
        {
            List<FilmInCinema> searchResult = null;
            if (page == 0 || pageSize == 0)
            {
                page = 1;
                pageSize = 1000;
            }
                using (var context = new CinemaManagementContext())
                {
                    IEnumerable<FilmInCinema> searchValues = await (from type in context.FilmInCinemas
                                                           where type.CinemaId.Equals(CinemaId)
                                                           select type).ToListAsync();
                    searchValues = searchValues.Skip((page - 1) * pageSize).Take(pageSize);
                    searchResult =  searchValues.ToList();
                }        

            return searchResult;
        }
        public async Task<List<FilmInCinema>> SearchByFilmId(int FilmId, int page, int pageSize)
        {
            List<FilmInCinema> searchResult = null;
            if (page == 0 || pageSize == 0)
            {
                page = 1;
                pageSize = 1000;
            }        
                using (var context = new CinemaManagementContext())
                {
                    IEnumerable<FilmInCinema> searchValues = await (from type in context.FilmInCinemas
                                                           where type.FilmId.Equals(FilmId)
                                                           select type).ToListAsync();
                    searchValues = searchValues.Skip((page - 1) * pageSize).Take(pageSize);
                    searchResult = searchValues.ToList();
                }
            return searchResult;
        }
        public static async Task<List<Film>> GetAllFilmInCinema(int CinemaId, int page, int pageSize)
        {
            var searchResult = new List<Film>();
            List<Film> searchResult1 = null;

            // IEnumerable<MemberObject> searchResult = null;
            try
            {
                if (page == 0 || pageSize == 0)
                {
                    page = 1;
                    pageSize = 1000;
                }
                using (var context = new CinemaManagementContext())
                {

                    IEnumerable<Film> searchValues = await (from film in context.Films
                                                       where film.FilmInCinemas.Any(c => c.CinemaId == CinemaId)
                                                       select film).ToListAsync();
                    searchValues =  searchValues.Skip((page - 1) * pageSize).Take(pageSize);
                    searchResult1 =  searchValues.ToList();
                    // searchValues =  searchValues.Skip((page - 1) * pageSize).Take(pageSize);

                }
                return searchResult1;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }
        public static async Task<List<Cinema>> GetAllCinemaHaveFilm(int FilmId, int page, int pageSize)
        {
            var searchResult = new List<Cinema>();
            List<Cinema> searchResult1 = null;

            // IEnumerable<MemberObject> searchResult = null;
            try
            {
                if (page == 0 || pageSize == 0)
                {
                    page = 1;
                    pageSize = 1000;
                }
                using (var context = new CinemaManagementContext())
                {

                    IEnumerable<Cinema> searchValues = await (from film in context.Cinemas
                                       where film.FilmInCinemas.Any(c => c.FilmId == FilmId)
                                       select film).ToListAsync();
                    searchValues = searchValues.Skip((page - 1) * pageSize).Take(pageSize);
                    searchResult1 =   searchValues.ToList();
                }

                return searchResult1;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }
       
    }
}

