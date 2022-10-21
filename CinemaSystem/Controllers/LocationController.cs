using Microsoft.AspNetCore.Mvc;
using BusinessObject.Models;
using DataAccess.Repository;
using DataAccess.DAO;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using CinemaSystem.Models;

namespace CinemaSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly ILocationRepository locationRepository;
        private readonly IConfiguration configuration;
        public LocationController(ILocationRepository _locationRepository, IConfiguration configuration)
        {
            locationRepository = _locationRepository;
            this.configuration = configuration;

        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {

            try
            {
                var AccountList = await locationRepository.GetLocations();
                var Count = AccountList.Count();
                return Ok(new { StatusCode = 200, Message = "Load successful", data = AccountList, Count });

            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }


        }
        [HttpGet("{id}")]
        public async Task<ActionResult> Getaccount(int id)
        {
            try
            {
                var Result = await locationRepository.GetLocationById(id);
                return Ok(new { StatusCode = 200, Message = "Load successful", data = Result });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }
        }
    }
}
