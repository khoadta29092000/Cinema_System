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
    public class TypeController : ControllerBase
    {
        private readonly ITypeRepository typeRepository;
        private readonly IConfiguration configuration;
        public TypeController(ITypeRepository _typeRepository, IConfiguration configuration)
        {
            typeRepository = _typeRepository;
            this.configuration = configuration;

        }


        [HttpGet]
        public async Task<IActionResult> GetAll(string search, int page, int pageSize)
        {

            try
            {
                var TypeList = await typeRepository.SearchByTitle(search, page, pageSize);
                var Count = TypeList.Count();
                return Ok(new { StatusCode = 200, Message = "Load successful", data = TypeList, Count });

            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }


        }
        [HttpGet("{id}")]
        public async Task<ActionResult> GetTypeById(int id)
        {
            try
            {
                var Result = await typeRepository.GetTypeById(id);
                return Ok(new { StatusCode = 200, Message = "Load successful", data = Result });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }
        }
    
        [HttpPost]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Create(Types type)
        {

            try
            {
                var newType = new Types
                {
                    Active = type.Active,
 
                    Title = type.Title,
                    Description = type.Description
                };
                await typeRepository.AddType(newType);
                return Ok(new { StatusCode = 200, Message = "Add successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }


        }
      
        [HttpPut("{id}")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> update(int id,TypeVM type)
        {
            if (id != type.Id)
            {
                return BadRequest();
            }
            try
            {
                using (var dbContext = new CinemaManagementContext())
                {
                    bool isDuplicateName = dbContext.Types
                        .Where(cnm => cnm.Id != type.Id)
                        .Any(cnm => String.Compare(cnm.Title, type.Title) == 0);
                    if (isDuplicateName) throw new Exception("Duplicate Name Of room");

                    var UnUpdatedModel = dbContext.Types.Find(type.Id);
                    if (UnUpdatedModel != null)
                    {
                        UnUpdatedModel.Active = type.Active;
                        UnUpdatedModel.Id = type.Id;
                        UnUpdatedModel.Title = type.Title;
                        UnUpdatedModel.Description = type.Description;

                        await dbContext.SaveChangesAsync();

                        return Ok(new { StatusCode = 200, Message = "Update successful" });
                    }
                    else
                    {
                        throw new Exception("Type Id Not Found");
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
                    Types type = await typeRepository.GetTypeById(id);
                    if (type == null)
                    {
                        return Ok(new { StatusCode = 400, Message = "Id not Exists" });

                    }
                    else
                    {
                        await typeRepository.UpdateActive(id, type.Active);
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

                await typeRepository.DeleteType(id);

                return Ok(new { StatusCode = 200, Message = "Delete successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }


        }
    }
}
