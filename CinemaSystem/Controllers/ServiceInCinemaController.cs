using Microsoft.AspNetCore.Mvc;
using BusinessObject.Models;
using DataAccess.Repository;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using CinemaSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceInCinemaController : ControllerBase
    {
        private readonly IServiceRepository serviceRepository;
        private readonly IServiceInCinemaRepository  serviceInCinemaRepository;
        private readonly IConfiguration configuration;
        public ServiceInCinemaController(IServiceRepository _serviceRepository, IServiceInCinemaRepository _serviceInCinemaRepository, IConfiguration configuration)
        {
            serviceInCinemaRepository = _serviceInCinemaRepository;
            this.configuration = configuration;
            serviceRepository = _serviceRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int cinemaId, int page , int pagesize)
        {

            try
            {
                var TypeList = await serviceInCinemaRepository.SearchByCinemaId(cinemaId,page,pagesize);
                var Count = TypeList.Count();
                return Ok(new { StatusCode = 200, Message = "Load successful", data = TypeList, Count });

            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }
        }
        [HttpGet("AllServiceInCinema")]
        public async Task<IActionResult> GetAllFilmInCinema(int CinemaId, int page, int pageSize)
        {

            try
            {
                var TypeList = await serviceInCinemaRepository.GetAllServiceInCinema(CinemaId, page, pageSize);
                var Count = TypeList.Count();
                return Ok(new { StatusCode = 200, Message = "Load successful", data = TypeList, Count });

            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }
        }
        [HttpGet("AllCinemaHaveService")]
        public async Task<IActionResult> GetAllCinemaHaveFilm(int ServiceId, int page, int pageSize)
        {

            try
            {
                var TypeList = await serviceInCinemaRepository.GetAllCinemaHaveService(ServiceId, page, pageSize);
                var Count = TypeList.Count();
                return Ok(new { StatusCode = 200, Message = "Load successful", data = TypeList, Count });

            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }
        }

        [HttpGet("AllServiceNotInCinema/{cinemaId}")]
        public async Task<IActionResult> GetAllServiceNotInCinema(int cinemaId, int page, int pageSize)
        {
            try
            {
                using (var dbContext = new CinemaManagementContext())
                {
                    var listServiceInCinema = dbContext.ServiceInCinemas.Where(f => f.CinemaId == cinemaId).Select(f => f.ServiceId).Distinct().ToList();
                    if (listServiceInCinema.Count == 0)
                    {
                        return Ok(new { StatusCode = 200, Message = "Load successful", data = dbContext.Services });
                    }
                    var listServiceNotInCinema = dbContext.Services.Where(f => !listServiceInCinema.Contains(f.Id)).ToList();
                    return Ok(new { StatusCode = 200, Message = "Load successful", data = listServiceNotInCinema });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult> GetTypeInFilmById(int Id)
        {
            try
            {
                var Result = await serviceInCinemaRepository.GetServiceInCinemaById(Id);
                return Ok(new { StatusCode = 200, Message = "Load successful", data = Result });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }
        }
        [HttpPost]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Create(ServiceInCinema serviceInCinema)
        {
            try
            {
                var newServiceInCinema = new ServiceInCinema
                {
                    ServiceId = serviceInCinema.ServiceId,
                    CinemaId = serviceInCinema.CinemaId,
                    Quantity = serviceInCinema.Quantity,
                    Active = serviceInCinema.Active
                };
                Service service = await serviceRepository.GetServiceById(serviceInCinema.ServiceId);
                int newQuantity = service.Quantity - serviceInCinema.Quantity ?? default(int);
                await serviceRepository.UpdateQuantity(service.Id, newQuantity);
                await serviceInCinemaRepository.AddServiceInCinema(newServiceInCinema);
                return Ok(new { StatusCode = 200, Message = "Add successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> update(int id, ServiceInCinema serviceInCinema)
        {
            if (id != serviceInCinema.Id)
            {
                return BadRequest();
            }
            try
            {
                var updateServiceInCinema = new ServiceInCinema
                {
                    Id = serviceInCinema.Id,
                    ServiceId = serviceInCinema.ServiceId,
                    CinemaId = serviceInCinema.CinemaId,
                    Quantity = serviceInCinema.Quantity,
                    Active = serviceInCinema.Active
                };
                await serviceInCinemaRepository.UpdateServiceInCinema(updateServiceInCinema);
                return Ok(new { StatusCode = 200, Message = "Update successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }

        }
        [HttpPut("UpdateQuantity")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> updateQuantity(int id, Quantity quantity)
        {

            try
            {
                Service service = await serviceRepository.GetServiceById(quantity.ServiceId);
                ServiceInCinema serviceInCinema = await serviceInCinemaRepository.GetServiceInCinemaById(id);
                int newQuantityInCinema = serviceInCinema.Quantity + quantity.quantity ?? default(int);
                int newQuantity = service.Quantity - quantity.quantity ?? default(int);
                await serviceRepository.UpdateQuantity(service.Id, newQuantity);
                await serviceInCinemaRepository.UpdateQuantity(id, newQuantityInCinema);
                
                return Ok(new { StatusCode = 200, Message = "Update successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }

        }

        [HttpPut("UpdateActive")]
            [Authorize(Roles = "1")]
            public async Task<IActionResult> updateActive(int id)
            {

            try
            {
                if (id == 0)
                {
                    return Ok(new { StatusCode = 400, Message = "Id is not Exits" });
                }
                else
                {
                    ServiceInCinema serviceInCinema = await serviceInCinemaRepository.GetServiceInCinemaById(id);
                    if (serviceInCinema == null)
                    {
                        return Ok(new { StatusCode = 400, Message = "Id not Exists" });

                    }
                    else
                    {
                        await serviceInCinemaRepository.UpdateActive(id, serviceInCinema.Active);
                        return Ok(new { StatusCode = 200, Message = "Update Active successful" });
                    }
                }

            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }


        }

        [HttpDelete("{Id}")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Delete(int Id)
        {

            try
            {

                await serviceInCinemaRepository.DeleteServiceInCinema(Id);

                return Ok(new { StatusCode = 200, Message = "Delete successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }


        }
    }
}
