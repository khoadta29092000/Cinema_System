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
    public class SchedulingController : ControllerBase
    {
        private readonly ISchedulingRepository schedulingRepository;
        private readonly IFilmRepository filmRepository;
        private readonly IRoomRepository roomRepository;
        private readonly IConfiguration configuration;
        public SchedulingController(IRoomRepository _roomRepository, ISchedulingRepository _schedulingRepository,  IConfiguration configuration, IFilmRepository _filmRepository)
        {
            roomRepository = _roomRepository;
            schedulingRepository = _schedulingRepository;
            this.configuration = configuration;
            filmRepository = _filmRepository;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll(DateTime Startdate, DateTime EndDate, int RoomId, int CinemaId, int FilmId, int page, int pageSize)
        {

            try
            {
                var TypeList = await schedulingRepository.FilterScheduling(Startdate, EndDate, RoomId, CinemaId, FilmId, page, pageSize);
                var Count = TypeList.Count();
                return Ok(new { StatusCode = 200, Message = "Load successful", data = TypeList, Count });

            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }


        }
        [HttpGet("{id}")]
        public async Task<ActionResult> GetSchedulingById(int id)
        {
            try
            {
                var Result = await schedulingRepository.GetSchedulingById(id);
                return Ok(new { StatusCode = 200, Message = "Load successful", data = Result });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }
        }

        [HttpPost]
        //[Authorize(Roles = "1")]
        public async Task<IActionResult> Create(Scheduling scheduling)
        {

            try
            {
                int filmId = scheduling.FilmId ?? default(int); 
                Film film = await filmRepository.GetFilmById(filmId);       
                TimeSpan StartTime = scheduling.StartTime ?? default(TimeSpan);
                int range = film.Time + 15 ?? default(int);
                TimeSpan duration = new TimeSpan(0, range, 0);
                TimeSpan EndTime = StartTime.Add(duration);
                 var room = await roomRepository.GetRoomById(scheduling.RoomId ?? default(int));
                var newScheduling = new Scheduling
                {
                    Active = scheduling.Active,
                    FilmId = scheduling.FilmId,
                    CinemaId = room.CinemaId,
                    Date = scheduling.Date,
                    EndTime = EndTime,
                    RoomId  = scheduling.RoomId,
                    StartTime = scheduling.StartTime,
                };
                await schedulingRepository.AddScheduling(newScheduling);
                return Ok(new { StatusCode = 200, Message = "Add successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }


        }

        [HttpPut("{id}")]
        //[Authorize(Roles = "1")]
        public async Task<IActionResult> update(int id, Scheduling scheduling)
        {
            if (id != scheduling.Id)
            {
                return BadRequest();
            }
            try
            {
                int filmId = scheduling.FilmId ?? default(int);
                Film film = await filmRepository.GetFilmById(filmId);
                TimeSpan StartTime = scheduling.StartTime ?? default(TimeSpan);
                int range = film.Time + 15 ?? default(int);
                TimeSpan duration = new TimeSpan(0, range, 0);
                TimeSpan EndTime = StartTime.Add(duration);
                var room = await roomRepository.GetRoomById(scheduling.RoomId ?? default(int));
                var updateScheduling = new Scheduling
                {
                    Active = scheduling.Active,
                    FilmId = scheduling.FilmId,
                    CinemaId = room.CinemaId,
                    Date = scheduling.Date,
                    RoomId = scheduling.RoomId,
                    Id = scheduling.Id,
                    StartTime = scheduling.StartTime,
                    EndTime = EndTime,
                };
                await schedulingRepository.UpdateScheduling(updateScheduling);
                return Ok(new { StatusCode = 200, Message = "Update successful" });
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
                    Scheduling scheduling = await schedulingRepository.GetSchedulingById(id);
                    if (scheduling == null)
                    {
                        return Ok(new { StatusCode = 400, Message = "Id not Exists" });

                    }
                    else
                    {
                        await schedulingRepository.UpdateActive(id, scheduling.Active);
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

                await schedulingRepository.DeleteScheduling(id);

                return Ok(new { StatusCode = 200, Message = "Delete successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }


        }
    }
}
