using Microsoft.AspNetCore.Mvc;
using BusinessObject.Models;
using DataAccess.Repository;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using System.Collections.Generic;
using CinemaSystem.ViewModel;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore.Query.Internal;

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
        public SchedulingController(IRoomRepository _roomRepository, ISchedulingRepository _schedulingRepository, IConfiguration configuration, IFilmRepository _filmRepository)
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

        [HttpPost("{cinemaId}/{date}")]
        //[Authorize(Roles = "1")]
        public async Task<IActionResult> Create(int cinemaId, DateTime date, int filmId)
        {

            try
            {
                using (var dbContext = new CinemaManagementContext())
                {
<<<<<<< Updated upstream
                    Active = scheduling.Active,
                    FilmId = scheduling.FilmId,
                    CinemaId = room.CinemaId,
                    Date = scheduling.Date,
                    EndTime = EndTime,
                    RoomId  = scheduling.RoomId,
                    StartTime = scheduling.StartTime,
                };
                await schedulingRepository.AddScheduling(newScheduling);
=======
                    if(dbContext.Schedulings.Where(s => s.CinemaId == cinemaId).Where(s => s.Date == date).Count() > 0) return StatusCode(409, new { StatusCode = 409, Message = "Already Had Scheduling for date " + date });
                    var rand = new Random();
                    var schedulings = dbContext.Schedulings.Where(sche => sche.CinemaId == cinemaId && sche.Date == date).ToList();
                    var filmInCinemaInDate = dbContext.FilmInCinemas.Where(f => f.CinemaId == cinemaId).Where(fic => fic.Startime <= date && date <= fic.Endtime).Select(x => x.FilmId).ToList();
                    TimeSpan lastHourOfDate = new TimeSpan(24, 0, 0);
                    var roomIdCount = dbContext.Rooms.Where(r => r.CinemaId == cinemaId).Select(rm => rm.Id).ToList();
                    if (filmInCinemaInDate.Count <= 0) return StatusCode(409, new { StatusCode = 409, Message = "No Film In " + date });
                    foreach (int roomId in roomIdCount)
                    {
                        while(TimeSpan.Compare(getLastestEndTime(roomId, schedulings), lastHourOfDate) == -1)
                        {
                            TimeSpan startTime = getLastestEndTime(roomId, schedulings);
                            var randomFilmId = rand.Next(0, filmInCinemaInDate.Count());
                            TimeSpan endTime = startTime.Add(new TimeSpan(0, dbContext.Films.Find(filmInCinemaInDate[randomFilmId]).Time + 15 ?? default(int), 0));
                            if (TimeSpan.Compare(endTime, lastHourOfDate) == 1) break;
                            schedulings = dbContext.Schedulings.Where(sche => sche.CinemaId == cinemaId && sche.Date == date).ToList();
                            var newScheduling = new Scheduling
                            {
                                Active = true,
                                FilmId = filmInCinemaInDate[randomFilmId],
                                CinemaId = cinemaId,
                                Date = date,
                                StartTime = startTime,
                                EndTime = endTime,
                                RoomId = roomId,
                            };

                            dbContext.Schedulings.Add(newScheduling);
                            await dbContext.SaveChangesAsync();
                        }
                    }
                    
                }
>>>>>>> Stashed changes
                return Ok(new { StatusCode = 200, Message = "Add successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }
        }

        private TimeSpan getLastestEndTime(int roomId, List<Scheduling> schedulings)
        {
<<<<<<< Updated upstream
            if (id != scheduling.Id)
            {
                return BadRequest();
            }
            try
            {
                int filmId = scheduling.FilmId ?? default(int);
                Film film = await filmRepository.GetFilmById(filmId);
                TimeSpan StartTime = scheduling.StartTime;
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


=======
            if (schedulings.Where(sch => sch.RoomId == roomId).ToList().Count() == 0) return new TimeSpan(7, 30, 0);
            else return schedulings.Where(sch => sch.RoomId == roomId).ToList().Max(sc => sc.EndTime);
>>>>>>> Stashed changes
        }

        //[HttpPost]
        ////[Authorize(Roles = "1")]
        //public async Task<IActionResult> Create(Scheduling scheduling)
        //{

        //    try
        //    {
        //        int filmId = scheduling.FilmId ?? default(int); 
        //        Film film = await filmRepository.GetFilmById(filmId);
        //        TimeSpan StartTime = scheduling.StartTime;
        //        int range = film.Time + 15 ?? default(int);
        //        TimeSpan duration = new TimeSpan(0, range, 0);
        //        TimeSpan EndTime = StartTime.Add(duration);
        //         var room = await roomRepository.GetRoomById(scheduling.RoomId ?? default(int));
        //        var newScheduling = new Scheduling
        //        {
        //            Active = scheduling.Active,
        //            FilmId = scheduling.FilmId,
        //            CinemaId = room.CinemaId,
        //            Date = scheduling.Date,
        //            EndTime = scheduling.EndTime,
        //            RoomId  = scheduling.RoomId,
        //            StartTime = scheduling.StartTime,
        //        };
        //        await schedulingRepository.AddScheduling(newScheduling);
        //        return Ok(new { StatusCode = 200, Message = "Add successful" });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
        //    }
        //}

        //[HttpPut("{id}")]
        ////[Authorize(Roles = "1")]
        //public async Task<IActionResult> update(int id, Scheduling scheduling)
        //{
        //    if (id != scheduling.Id)
        //    {
        //        return BadRequest();
        //    }
        //    try
        //    {
        //        int filmId = scheduling.FilmId ?? default(int);
        //        Film film = await filmRepository.GetFilmById(filmId);
        //        TimeSpan StartTime = scheduling.StartTime;
        //        int range = film.Time + 15 ?? default(int);
        //        TimeSpan duration = new TimeSpan(0, range, 0);
        //        TimeSpan EndTime = StartTime.Add(duration);
        //        var room = await roomRepository.GetRoomById(scheduling.RoomId ?? default(int));
        //        var updateScheduling = new Scheduling
        //        {
        //            Active = scheduling.Active,
        //            FilmId = scheduling.FilmId,
        //            CinemaId = room.CinemaId,
        //            Date = scheduling.Date,
        //            RoomId = scheduling.RoomId,
        //            Id = scheduling.Id,
        //            StartTime = scheduling.StartTime,
        //            EndTime = scheduling.EndTime,
        //        };
        //        await schedulingRepository.UpdateScheduling(updateScheduling);
        //        return Ok(new { StatusCode = 200, Message = "Update successful" });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
        //    }


        //}

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
