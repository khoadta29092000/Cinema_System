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
    public class SeatController : ControllerBase
    {
        private readonly ISeatRepository seatRepository;
        private readonly IConfiguration configuration;
        public SeatController(ISeatRepository _seatRepository, IConfiguration configuration)
        {
            seatRepository = _seatRepository;
            this.configuration = configuration;

        }
        [HttpGet]
        public async Task<IActionResult> GetAll(string search, int page, int pageSize)
        {

            try
            {
                var TypeList = await seatRepository.SearchByTitle(search, page, pageSize);
                var Count = TypeList.Count();
                return Ok(new { StatusCode = 200, Message = "Load successful", data = TypeList, Count });

            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }


        }
        [HttpGet("{id}")]
        public async Task<ActionResult> GetRoomById(int id)
        {
            try
            {
                var Result = await seatRepository.GetSeatById(id);
                return Ok(new { StatusCode = 200, Message = "Load successful", data = Result });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Create(Seat seat)
        {

            try
            {
                var newSeat = new Seat
                {
                    Active = seat.Active,
                    RoomId = seat.RoomId,
                    Title = seat.Title,
                    Description = seat.Description,
                    
                };
                await seatRepository.AddSeat(newSeat);
                return Ok(new { StatusCode = 200, Message = "Add successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }


        }

        [HttpPut("{id}")]
        //[Authorize(Roles = "1")]
        public async Task<IActionResult> update(int id, SeatVM seat)
        {
            if (id != seat.Id)
            {
                return BadRequest();
            }
            try
            {
                using (var dbContext = new CinemaManagementContext())
                {
                    bool isDuplicateName = dbContext.Seats
                        .Where(cnm => cnm.Id != seat.Id)
                        .Where(cnm => cnm.RoomId == seat.RoomId)
                        .Any(cnm => String.Compare(cnm.Title, seat.Title) == 0);
                    if (isDuplicateName) throw new Exception("Duplicate Name Of Seat");

                    var UnUpdatedModel = dbContext.Seats.Find(seat.Id);
                    if (UnUpdatedModel != null)
                    {
                        UnUpdatedModel.Active = seat.Active;
                        UnUpdatedModel.RoomId = seat.RoomId;
                        UnUpdatedModel.Title = seat.Title;
                        UnUpdatedModel.Description = seat.Description;
                        UnUpdatedModel.Id = seat.Id;

                        await dbContext.SaveChangesAsync();
                        return Ok(new { StatusCode = 200, Message = "Update successful" });
                    } else
                    {
                        throw new Exception("Room Id Not Found");
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
                    Seat seat = await seatRepository.GetSeatById(id);
                    if (seat == null)
                    {
                        return Ok(new { StatusCode = 400, Message = "Id not Exists" });

                    }
                    else
                    {
                        await seatRepository.UpdateActive(id, seat.Active);
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

                await seatRepository.DeleteSeat(id);
                return Ok(new { StatusCode = 200, Message = "Delete successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }


        }
    }
}
