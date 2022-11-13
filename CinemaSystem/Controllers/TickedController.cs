using Microsoft.AspNetCore.Mvc;
using BusinessObject.Models;
using DataAccess.Repository;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace TickedSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TickedController : ControllerBase
    {
        private readonly ITickedRepository tickedRepository;
        private readonly IConfiguration configuration;
        
        public TickedController(ITickedRepository _tickedRepository, IConfiguration configuration)
        {
            tickedRepository = _tickedRepository;
            this.configuration = configuration;

        }


        [HttpGet]
        public async Task<IActionResult> GetAll(int SeatId, int BillId, int SchedulingId, int page, int pageSize)
        {

            try
            {
                var TickedList = await tickedRepository.FilterTicked(SeatId,BillId,SchedulingId, page, pageSize);
                var Count = TickedList.Count();
                return Ok(new { StatusCode = 200, Message = "Load successful", data = TickedList, Count });

            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }


        }
       
        [HttpGet("SeatId/{SeatId}/BillId/{BillId}/SchedulingId/{SchedulingId}")]
        public async Task<ActionResult> GetTickedById(int SeatId, int BillId, int SchedulingId)
        {
            try
            {
                var Result = await tickedRepository.GetTickedById(SeatId,BillId,SchedulingId);
                return Ok(new { StatusCode = 200, Message = "Load successful", data = Result });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Create(Ticked Ticked)
        {

            try
            {
                var newTicked = new Ticked
                {
                    SchedulingId = Ticked.SchedulingId,
                    SeatId = Ticked.SeatId,
                    AccountId = Ticked.AccountId,
                    BillId = Ticked.BillId,
                    Price = Ticked.Price,
               
                };
                await tickedRepository.AddTicked(newTicked);
                return Ok(new { StatusCode = 200, Message = "Add successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }


        }

        [HttpPut("SeatId/{SeatId}/BillId/{BillId}/SchedulingId/{SchedulingId}")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> update(int SeatId, int BillId, int SchedulingId, Ticked Ticked)
        {
            if (SeatId != Ticked.SeatId && BillId != Ticked.BillId && SchedulingId != Ticked.SchedulingId)
            {
                return BadRequest();
            }
            try
            {
                var newTicked = new Ticked
                {
                    SchedulingId = Ticked.SchedulingId,
                    SeatId = Ticked.SeatId,
                    AccountId = Ticked.AccountId,
                    BillId = Ticked.BillId,
                    Price = Ticked.Price,

                };
                await tickedRepository.UpdateTicked(newTicked);
                return Ok(new { StatusCode = 200, Message = "Update successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }


        }

       

        [HttpDelete("SeatId/{SeatId}/BillId/{BillId}/SchedulingId/{SchedulingId}")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Delete(int SeatId, int BillId, int SchedulingId)
        {

            try
            {

                await tickedRepository.DeleteTicked(SeatId,BillId,SchedulingId);

                return Ok(new { StatusCode = 200, Message = "Delete successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }


        }

    }
}
