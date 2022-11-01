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
    public class RoomController : ControllerBase
    {
        private readonly IRoomRepository roomRepository;
        private readonly IConfiguration configuration;
        public RoomController(IRoomRepository _roomRepository, IConfiguration configuration)
        {
            roomRepository = _roomRepository;
            this.configuration = configuration;

        }
        [HttpGet]
        public async Task<IActionResult> GetAll(string search, int CinemaId, int page, int pageSize)
        {

            try
            {
                var TypeList = await roomRepository.SearchByTitle(search, CinemaId, page, pageSize);
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
                var Result = await roomRepository.GetRoomById(id);
                return Ok(new { StatusCode = 200, Message = "Load successful", data = Result });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Create(Room room)
        {

            try
            {
                var newRoom = new Room
                {
                    Active = room.Active,
                    CinemaId = room.CinemaId,
                    Title = room.Title,
                    Description = room.Description,                  
                };
                await roomRepository.AddRoom(newRoom);
                return Ok(new { StatusCode = 200, Message = "Add successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }

        }

        [HttpPut("{id}")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> update(int id, RoomVM room)
        {
            if (id != room.Id)
            {
                return BadRequest();
            }
            try
            {
                using (var dbContext = new CinemaManagementContext())
                {
                    bool isDuplicateName = dbContext.Rooms
                        .Where(cnm => cnm.Id != room.Id)
                        .Where(cnm => cnm.CinemaId == room.CinemaId)
                        .Any(cnm => String.Compare(cnm.Title, room.Title) == 0);
                    if (isDuplicateName) throw new Exception("Duplicate Name Of room");

                    var UnUpdatedModel = dbContext.Rooms.Find(room.Id);
                    if (UnUpdatedModel != null)
                    {
                        UnUpdatedModel.Id = room.Id;
                        UnUpdatedModel.Active = room.Active;
                        UnUpdatedModel.CinemaId = room.CinemaId;
                        UnUpdatedModel.Title = room.Title;
                        UnUpdatedModel.Description = room.Description;

                        await dbContext.SaveChangesAsync();

                        return Ok(new { StatusCode = 200, Message = "Update successful" });
                    }
                    else
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
                    RoomDTO room = await roomRepository.GetRoomById(id);
                    if (room == null)
                    {
                        return Ok(new { StatusCode = 400, Message = "Id not Exists" });

                    }
                    else
                    {
                        await roomRepository.UpdateActive(id, room.Active);
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

                await roomRepository.DeleteRoom(id);
                return Ok(new { StatusCode = 200, Message = "Delete successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }


        }
    }
}
