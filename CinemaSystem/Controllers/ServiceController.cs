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
    public class ServiceController : ControllerBase
    {
        private readonly IServiceRepository serviceRepository;
        private readonly IConfiguration configuration;
        public ServiceController(IServiceRepository _serviceRepository, IConfiguration configuration)
        {
            serviceRepository = _serviceRepository;
            this.configuration = configuration;

        }


        [HttpGet]
        public async Task<IActionResult> GetAll(string search, int page, int pageSize)
        {

            try
            {
                var ServiceList = await serviceRepository.SearchByTitle(search, page, pageSize);
                var Count = ServiceList.Count();
                return Ok(new { StatusCode = 200, Message = "Load successful", data = ServiceList, Count });

            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }


        }
        [HttpGet("{id}")]
        public async Task<ActionResult> GetServiceById(int id)
        {
            try
            {
                var Result = await serviceRepository.GetServiceById(id);
                return Ok(new { StatusCode = 200, Message = "Load successful", data = Result });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Create(Service service)
        {

            try
            {
                var newService = new Service
                {
                    Active = service.Active,
                    Title = service.Title,
                    Description = service.Description,
                    Price = service.Price,
                    Quantity = service.Quantity,
                    Image = service.Image
                };
                await serviceRepository.AddService(newService);
                return Ok(new { StatusCode = 200, Message = "Add successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }


        }

        [HttpPut("{id}")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> update(int id,Service service)
        {
            if (id != service.Id)
            {
                return BadRequest();
            }
            try
            {
                var updateService = new Service
                {
                    Active = service.Active,
                    Title = service.Title,
                    Description = service.Description,
                    Price = service.Price,
                    Quantity = service.Quantity,
                    Id = service.Id,
                    Image = service.Image
                };
                await serviceRepository.UpdateService(updateService);
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
                    Service service = await serviceRepository.GetServiceById(id);
                    if (service == null)
                    {
                        return Ok(new { StatusCode = 400, Message = "Id not Exists" });

                    }
                    else
                    {
                        await serviceRepository.UpdateActive(id, service.Active);
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

                await serviceRepository.DeleteService(id);

                return Ok(new { StatusCode = 200, Message = "Delete successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }


        }
    }
}
