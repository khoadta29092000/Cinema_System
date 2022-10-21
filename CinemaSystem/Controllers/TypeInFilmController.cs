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
    public class TypeInFilmController : ControllerBase
    {
        private readonly ITypeInFilmRepository typeInFilmRepository;
        private readonly IConfiguration configuration;
        public TypeInFilmController(ITypeInFilmRepository _typeInFilmRepository, IConfiguration configuration)
        {
            typeInFilmRepository = _typeInFilmRepository;
            this.configuration = configuration;

        }


        [HttpGet]
        public async Task<IActionResult> GetAll(int FilmId, int page, int pageSize)
        {

            try
            {
                var TypeList = await typeInFilmRepository.SearchByTypeId(FilmId, page, pageSize);
                var Count = TypeList.Count();
                return Ok(new { StatusCode = 200, Message = "Load successful", data = TypeList, Count });

            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }


        }
        [HttpGet("{TypeId},{FilmId}")]
        public async Task<ActionResult> GetTypeInFilmById(int TypeId, int FilmId)
        {
            try
            {
                var Result = await typeInFilmRepository.GetTypeInFilmById(TypeId, FilmId);
                return Ok(new { StatusCode = 200, Message = "Load successful", data = Result });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }
        }
       /* [HttpGet("TypeInFilm")]
        public async Task<ActionResult> GetTypeInFilm(int FilmId)
        {
            try
            {
                var Result = await typeInFilmRepository.GetTypeInFilm(FilmId);
                return Ok(new { StatusCode = 200, Message = "Load successful", data = Result });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }
        } */

        [HttpPost]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Create(TypeInFilm typeInFilm)
        {
            try
            {
                var newTypeInFilm = new TypeInFilm
                {
                    Active = typeInFilm.Active,
                    FilmId = typeInFilm.FilmId,
                    TypeId = typeInFilm.TypeId
                };
               
                await typeInFilmRepository.AddTypeInFilm(newTypeInFilm);
                return Ok(new { StatusCode = 200, Message = "Add successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }
        }

        [HttpPut("{TypeId},{FilmId}")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> update(int FilmId, int TypeId, TypeInFilm typeInFilm)
        {
            if (FilmId != typeInFilm.FilmId && TypeId != typeInFilm.TypeId)
            {
                return BadRequest();
            }
            try
            {
                var updateTypeInFilm = new TypeInFilm
                {
                    Active = typeInFilm.Active,
                    FilmId = typeInFilm.FilmId,
                    TypeId = typeInFilm.TypeId
                };
                await typeInFilmRepository.UpdateTypeInFilm(updateTypeInFilm);
                return Ok(new { StatusCode = 200, Message = "Update successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }


        }

        [HttpDelete("{TypeId},{FilmId}")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Delete(int TypeId, int FilmId)
        {

            try
            {

                await typeInFilmRepository.DeleteTypeInFilm(TypeId, FilmId);

                return Ok(new { StatusCode = 200, Message = "Delete successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }


        }
        [HttpPut("UpdateActive")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> UpdateActive(int FilmId,int TypeId)
        {

            try
            {
                if (FilmId == 0 && TypeId == 0)
                {
                    return Ok(new { StatusCode = 400, Message = "Id is not Exits" });
                }
                else
                {
                    TypeInFilm typeInFilm = await typeInFilmRepository.GetTypeInFilmById(TypeId, FilmId);
                    if (typeInFilm == null)
                    {
                        return Ok(new { StatusCode = 400, Message = "Id not Exists" });

                    }
                    else
                    {
                        await typeInFilmRepository.UpdateActive(TypeId, FilmId, typeInFilm.Active);
                        return Ok(new { StatusCode = 200, Message = "Update Active successful" });
                    }
                }

            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }


        }
    }
}
