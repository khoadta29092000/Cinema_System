using Microsoft.AspNetCore.Mvc;
using BusinessObject.Models;
using DataAccess.Repository;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using CinemaSystem.Models;
using CinemaSystem.ViewModel;

namespace CinemaSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilmController : ControllerBase
    {
        private readonly IFilmRepository filmRepository;
        private readonly ITypeInFilmRepository typeInFilmRepository;
        private readonly IConfiguration configuration;
        public FilmController(IFilmRepository _filmRepository, ITypeInFilmRepository _typeInFilmRepository, IConfiguration configuration)
        {
            filmRepository = _filmRepository;
            this.configuration = configuration;
            typeInFilmRepository = _typeInFilmRepository;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll(string search, int page, int pageSize)
        {

            try
            {
                var TypeList = await filmRepository.SearchByTitle(search, page, pageSize);
                var Count = TypeList.Count();
                return Ok(new { StatusCode = 200, Message = "Load successful", data = TypeList, Count });

            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }


        }
        [HttpGet("{id}")]
        public async Task<ActionResult> GetFilmById(int id)
        {
            try
            {
                var Result = await filmRepository.GetFilmById(id);
                return Ok(new { StatusCode = 200, Message = "Load successful", data = Result });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Create(FilmObject film)
        {

            try
            {
                Film newFilm = new Film
                {
                    Active = film.Active,
                    Title = film.Title,
                    Description = film.Description,
                    Actor = film.Actor,
                    Director = film.Director,
                    Language = film.Language,
                    Rated = film.Rated,
                    Time = film.Time,
                    Trailer = film.Trailer,
                    Image = film.Image,
                };
                await filmRepository.AddFilm(newFilm);
                var filmList = await filmRepository.GetFilms();
                Film idFilm = filmList.LastOrDefault(x => x.Id != null);
                foreach (int item in film.TypeInFilm)
                {
                    var newType = new TypeInFilm
                    {
                        FilmId = idFilm.Id,
                        TypeId = item,
                        Active = true
                    };
                    await typeInFilmRepository.AddTypeInFilm(newType);
                }
                return Ok(new { StatusCode = 200, Message = "Add successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }


        }

        [HttpPut("{id}")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> update(int id, FilmVM film)
        {
            if (id != film.Id)
            {
                return BadRequest();
            }
            try
            {
                using (var dbContext = new CinemaManagementContext())
                {
                    bool isDuplicateName = dbContext.Films
                        .Where(cnm => cnm.Id != film.Id)
                        .Any(cnm => String.Compare(cnm.Title, film.Title) == 0);
                    if (isDuplicateName) throw new Exception("Duplicate Name Of Film");

                    var UnUpdatedModel = dbContext.Films.Find(film.Id);
                    if (UnUpdatedModel != null) 
                    {
                        //if (UnUpdatedModel.TypeInFilms.ToList().Count > 0)
                        //{
                        //    UnUpdatedModel.TypeInFilms.Select(tif => tif.TypeId)
                        //        .Where(tid => !film.TypeInFilms.Select(obj => obj.TypeId).Contains(tid))
                        //        .Select(rmt => dbContext.TypeInFilms
                        //            .Remove(new TypeInFilm() 
                        //            { TypeId = rmt, 
                        //                FilmId = UnUpdatedModel.Id 
                        //            }));

                        //    film.TypeInFilms.Select(tif => tif.TypeId)
                        //        .Where(tid => !UnUpdatedModel.TypeInFilms.Select(obj => obj.TypeId).Contains(tid))
                        //        .Select(rmt => dbContext.TypeInFilms
                        //            .Add(new TypeInFilm()
                        //            {
                        //                TypeId = rmt,
                        //                FilmId = UnUpdatedModel.Id
                        //            }));
                        //}   

                        //UnUpdatedModel.Active = film.Active;
                        //UnUpdatedModel.Title = film.Title;
                        //UnUpdatedModel.Description = film.Description;
                        //UnUpdatedModel.Actor = film.Actor;
                        //UnUpdatedModel.Director = film.Director;
                        //UnUpdatedModel.Language = film.Language;
                        //UnUpdatedModel.Rated = film.Rated;
                        //UnUpdatedModel.Time = film.Time;
                        //UnUpdatedModel.Trailer = film.Trailer;
                        //UnUpdatedModel.Id = film.Id;
                        //UnUpdatedModel.Image = film.Image;
                        //UnUpdatedModel.TypeInFilms = film.TypeInFilms;

                        //await dbContext.SaveChangesAsync();
                        return Ok(new { StatusCode = 200, Message = "Update successful" });
                    }
                    else
                    {
                        throw new Exception("Film Id Not Found");
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }

        }

        [HttpPut("UpdateActive")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> UpdateActive(int id)
        {

            try
            {
                if (id == 0)
                {
                    return Ok(new { StatusCode = 400, Message = "Id is not Exits" });
                }
                else
                {
                    Film film = await filmRepository.GetFilmById(id);
                    if (film == null)
                    {
                        return Ok(new { StatusCode = 400, Message = "Id not Exists" });

                    }
                    else
                    {
                        await filmRepository.UpdateActive(id, film.Active);
                        return Ok(new { StatusCode = 200, Message = "Update Active successful" });
                    }
                }

            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }


        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Delete(int id)
        {

            try
            {

                await filmRepository.DeleteFilm(id);

                return Ok(new { StatusCode = 200, Message = "Delete successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }


        }
    }
}
