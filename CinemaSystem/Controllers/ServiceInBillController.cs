using DataAccess.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceInBillController : ControllerBase
    {
        private readonly IServiceInBillRepository serviceInBillRepository;
        private readonly IConfiguration configuration;

        public ServiceInBillController(IServiceInBillRepository _serviceInBillRepository, IConfiguration configuration)
        {
            serviceInBillRepository = _serviceInBillRepository;
            this.configuration = configuration;

        }


        [HttpGet]
        public async Task<IActionResult> GetAll(int BillId, int SchedulingId, int page, int pageSize)
        {

            try
            {
                var TickedList = await serviceInBillRepository.SearchByCinemaId( BillId, page, pageSize);
                var Count = TickedList.Count();
                return Ok(new { StatusCode = 200, Message = "Load successful", data = TickedList, Count });

            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }


        }
        [HttpGet("ServiceInBill")]
        public async Task<IActionResult> GetAll1()
        {

            try
            {
                var TickedList = await serviceInBillRepository.GetServiceInBills1();
                var Count = TickedList.Count();
                return Ok(new { StatusCode = 200, Message = "Load successful", data = TickedList, Count });

            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }


        }
    }
}
