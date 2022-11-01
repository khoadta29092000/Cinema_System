using Microsoft.AspNetCore.Mvc;
using BusinessObject.Models;
using DataAccess.Repository;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using CinemaSystem.ViewModel;

namespace CinemaSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CinemaController : ControllerBase
    {
        private readonly ICinemaRepository cinemaRepository;
        private readonly IConfiguration configuration;
        public CinemaController(ICinemaRepository _cinemaRepository, IConfiguration configuration)
        {
            cinemaRepository = _cinemaRepository;
            this.configuration = configuration;

        }


        [HttpGet]
        public async Task<IActionResult> GetAll(string search, int page, int pageSize)
        {

            try
            {
                var CinemaList = await cinemaRepository.SearchByName(search, page, pageSize);
                var Count = CinemaList.Count();
                return Ok(new { StatusCode = 200, Message = "Load successful", data = CinemaList, Count });

            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }


        }
       
        [HttpGet("{id}")]
        public async Task<ActionResult> GetCinemaById(int id)
        {
            try
            {
                var Result = await cinemaRepository.GetCinemaById(id);
                return Ok(new { StatusCode = 200, Message = "Load successful", data = Result });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Create(Cinema cinema)
        {

            try
            {
                var newCinema = new Cinema
                {
                    Active = cinema.Active,
                    Address = cinema.Address,
                    Name = cinema.Name,
                    Description = cinema.Description,
                    Image = cinema.Image,
                    LocationId = cinema.LocationId,
                    
                };
                await cinemaRepository.AddCinema(newCinema);
                return Ok(new { StatusCode = 200, Message = "Add successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }


        }

        [HttpPut("{id}")]
        //[Authorize(Roles = "1")]
        public async Task<IActionResult> update(int id,CinemaVM cinema)
        {
            if (id != cinema.Id)
            {
                return BadRequest();
            }
            try
            {
                using (var dbContext = new CinemaManagementContext())
                {
                    
                    bool isDuplicateName = dbContext.Cinemas
                        .Where(cnm => cnm.Id != cinema.Id)
                        .Any(cnm => String.Compare(cnm.Name, cinema.Name) == 0);
                    if (isDuplicateName) throw new Exception("Duplicate Name Of Cinema");

                    var UnUpdatedCinema = dbContext.Cinemas.Find(cinema.Id);
                    if (UnUpdatedCinema != null)
                    {
                        UnUpdatedCinema.Active = cinema.Active;
                        UnUpdatedCinema.Address = cinema.Address;
                        UnUpdatedCinema.Name = cinema.Name;
                        UnUpdatedCinema.Description = cinema.Description;
                        UnUpdatedCinema.Image = cinema.Image;
                        UnUpdatedCinema.LocationId = cinema.LocationId;
                        UnUpdatedCinema.Id = cinema.Id;

                        await dbContext.SaveChangesAsync();

                        return Ok(new { StatusCode = 200, Message = "Update successful" });
                    } else
                    {
                        throw new Exception("Cinema Id Not Found");
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
                    Cinema cinema = await cinemaRepository.GetCinemaById(id);
                    if (cinema == null)
                    {
                        return Ok(new { StatusCode = 400, Message = "Id not Exists" });

                    }
                    else
                    {
                        await cinemaRepository.UpdateActive(id, cinema.Active);
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

                await cinemaRepository.DeleteCinema(id);

                return Ok(new { StatusCode = 200, Message = "Delete successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }


        }
    }
}
