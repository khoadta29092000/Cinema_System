using Microsoft.AspNetCore.Mvc;
using BusinessObject.Models;
using DataAccess.Repository;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using CinemaSystem.ViewModel;

namespace CinemaSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilmInCinemaController : ControllerBase
    {
        private readonly IFilmInCinemaRepository filmInCinemaRepository;
        private readonly IConfiguration configuration;
        public FilmInCinemaController(IFilmInCinemaRepository _filmInCinemaRepository, IConfiguration configuration)
        {
            filmInCinemaRepository = _filmInCinemaRepository;
            this.configuration = configuration;

        }
        [HttpGet]
        public async Task<IActionResult> GetFilm()
        {

            try
            {
                var TypeList = await filmInCinemaRepository.GetFilmInCinemas();
                var Count = TypeList.Count();
                return Ok(new { StatusCode = 200, Message = "Load successful", data = TypeList, Count });

            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }
        }


        [HttpGet("AllFilmInCinema")]
        public async Task<IActionResult> GetAllFilmInCinema(int CinemaId, int page, int pageSize)
        {

            try
            {
                var TypeList = await filmInCinemaRepository.GetAllFilmInCinema(CinemaId, page, pageSize);
                var Count = TypeList.Count();
                return Ok(new { StatusCode = 200, Message = "Load successful", data = TypeList, Count });

            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }
        }

        [HttpGet("AllFilmInCinemaToday")]
        public async Task<IActionResult> GetAllFilmInCinemaToday(int page, int pageSize)
        {
            var filmInCinemaFromDB = new List<FilmInCinema>();
            try
            {
                using (var dbContext = new CinemaManagementContext())
                {
                    filmInCinemaFromDB = await dbContext.FilmInCinemas.ToListAsync();
                    var listFilm = filmInCinemaFromDB
                        .Where(f => f.Startime.Value.Date <= DateTime.Now.Date && DateTime.Now.Date <= f.Endtime.Value.Date).ToList()
                        .Select(fic => dbContext.Films.Find(fic.FilmId)).GroupBy(f => f.Id).Select(x => x.FirstOrDefault()).ToList();

             
                    return Ok(new { StatusCode = 200, Message = "Load successful", data = listFilm });

                }

            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }
        }

        [HttpGet("AllFilmInCinemaToday/{CinemaId}/{date}")]
        public async Task<IActionResult> GetAllFilmInCinemaTodayByCinemaId(int CinemaId, DateTime date, int page, int pageSize)
        {
            var filmInCinemaFromDB = new List<FilmInCinema>();
            try
            {
                if (date == null) date = DateTime.Now;
                using (var dbContext = new CinemaManagementContext())
                {
                    filmInCinemaFromDB = await dbContext.FilmInCinemas.ToListAsync();
                    //var filmInCinemaInDate = dbContext.FilmInCinemas.Where(f => f.CinemaId == cinemaId).Where(fic => fic.Startime <= date && date <= fic.Endtime).Select(x => x.FilmId).ToList();
                    var listFilm = filmInCinemaFromDB
                        .Where(f => f.CinemaId == CinemaId)
                        .Where(f => f.Startime.Value.Date <= date.Date && date.Date <= f.Endtime.Value.Date).ToList()
                        .Select(fic => dbContext.Films.Find(fic.FilmId))
                        .GroupBy(f => f.Id).Select(x => x.FirstOrDefault())
                        .Select(f => new FilmInCinemaVM() {
                            Id = f.Id,
                            Title = f.Title,
                            Director = f.Director,
                            Actor = f.Actor,
                            Time = f.Time,
                            Language = f.Language,
                            Rated = f.Rated,
                            Trailer = f.Trailer,
                            Description = f.Description,
                            Image = f.Image,
                            Active = f.Active,
                            Startime = f.FilmInCinemas.FirstOrDefault(obj => obj.CinemaId == CinemaId && obj.FilmId == f.Id).Startime,
                            Endtime = f.FilmInCinemas.FirstOrDefault(obj => obj.CinemaId == CinemaId && obj.FilmId == f.Id).Endtime
                        })
                        .ToList();

                    return Ok(new { StatusCode = 200, Message = "Load successful", data = listFilm });

                }

            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }
        }

        [HttpGet("AllFilmInCinemaComingSoon")]
        public async Task<IActionResult> GetAllFilmInCinemaComingSoon(int page, int pageSize)
        {
            var filmInCinemaFromDB = new List<FilmInCinema>();
            try
            {
                using (var dbContext = new CinemaManagementContext())
                {
                    filmInCinemaFromDB = await dbContext.FilmInCinemas.ToListAsync();
                    var listFilm = filmInCinemaFromDB
                        .Where(f => DateTime.Now.Date < f.Startime.Value.Date).ToList()
                        .Select(fic => dbContext.Films.Find(fic.FilmId)).GroupBy(f => f.Id).Select(x => x.FirstOrDefault()).ToList();

                    if (listFilm.Count <= 0) return Ok("There is no Film comming soon");
                    return Ok(new { StatusCode = 200, Message = "Load successful", data = listFilm });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }
        }

        [HttpGet("AllFilmNotInCinema/{cinemaId}")]
        public async Task<IActionResult> GetAllFilmNotInCinema(int cinemaId, int page, int pageSize)
        {
            try
            {
                using (var dbContext = new CinemaManagementContext())
                {
                    var listFilmInCinema = dbContext.FilmInCinemas.Where(f => f.CinemaId == cinemaId).Select(f => f.FilmId).Distinct().ToList();
                    if (listFilmInCinema.Count == 0)
                    {
                        return Ok(new { StatusCode = 200, Message = "Load successful", data = dbContext.Films });
                    }
                    var listFilmNotInCinema = dbContext.Films.Where(f => !listFilmInCinema.Contains(f.Id)).ToList();
                    return Ok(new { StatusCode = 200, Message = "Load successful", data = listFilmNotInCinema });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }
        }

        [HttpGet("AllCinemaHaveFilm")]
        public async Task<IActionResult> GetAllCinemaHaveFilm(int FilmId, int page, int pageSize)
        {

            try
            {
                var TypeList = await filmInCinemaRepository.GetAllCinemaHaveFilm(FilmId, page, pageSize);
                var Count = TypeList.Count();
                return Ok(new { StatusCode = 200, Message = "Load successful", data = TypeList, Count });

            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }
        }
        [HttpGet("{FilmId},{CinemaId}")]
        public async Task<ActionResult> GetTypeInFilmById(int CinemaId, int FilmId)
        {
            try
            {
                var Result = await filmInCinemaRepository.GetFilmInCinemaById(CinemaId, FilmId);
                return Ok(new { StatusCode = 200, Message = "Load successful", data = Result });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }
        }
        [HttpPost]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Create(FilmInCinema filmInCinema)
        {
            try
            {
                var newfilmInCinema = new FilmInCinema
                {          
                    FilmId = filmInCinema.FilmId,
                    CinemaId = filmInCinema.CinemaId,
                    Startime = filmInCinema.Startime,
                    Endtime = filmInCinema.Endtime
                };
                await filmInCinemaRepository.AddFilmInCinema(newfilmInCinema);
                return Ok(new { StatusCode = 200, Message = "Add successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }
        }

        [HttpPut("{FilmId}")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> update(int FilmId, FilmInCinema filmInCinema)
        {
            if (FilmId != filmInCinema.FilmId)
            {
                return BadRequest();
            }
            try
            {
                var updatefilmInCinema = new FilmInCinema
                {
                    FilmId = filmInCinema.FilmId,
                    CinemaId = filmInCinema.CinemaId,
                    Startime = filmInCinema.Startime,
                    Endtime = filmInCinema.Endtime
                };
                await filmInCinemaRepository.UpdateFilmInCinema(updatefilmInCinema);
                return Ok(new { StatusCode = 200, Message = "Update successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }


        }

        [HttpDelete("{FilmId}")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Delete(int CinemaId, int FilmId)
        {

            try
            {

                await filmInCinemaRepository.DeleteFilmInCinema(CinemaId, FilmId);

                return Ok(new { StatusCode = 200, Message = "Delete successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }


        }
    }
}
