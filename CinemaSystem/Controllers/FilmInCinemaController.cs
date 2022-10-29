using Microsoft.AspNetCore.Mvc;
using BusinessObject.Models;
using DataAccess.Repository;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;

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
        public async Task<IActionResult> GetAllFilmInCinemaToday(int CinemaId, int page, int pageSize)
        {
            var dbContext = new CinemaManagementContext();
            try
            {
                
                var TypeList = await filmInCinemaRepository.GetAllFilmInCinema(CinemaId, page, pageSize);
                var listFilm = dbContext.FilmInCinemas
                    .Where(x => x.CinemaId == CinemaId).ToList()
                    .Where(f => f.Startime.Value.Date <= DateTime.Now.Date && DateTime.Now.Date <= f.Endtime.Value.Date).ToList()
                    .Select(fic => { fic.Film = dbContext.Films.Find(fic.FilmId); return fic; }).ToList();
                
                if (listFilm.Count <= 0) return Ok("No Film for today");
                return Ok(listFilm);

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
