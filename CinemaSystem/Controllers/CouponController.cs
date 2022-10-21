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
    public class CouponController : ControllerBase
    {
        private readonly ICouponRepository couponRepository;
        private readonly IConfiguration configuration;
        public CouponController(ICouponRepository _couponRepository, IConfiguration configuration)
        {
            couponRepository = _couponRepository;
            this.configuration = configuration;

        }


        [HttpGet]
        public async Task<IActionResult> GetAll(string search, int page, int pageSize)
        {

            try
            {
                var TypeList = await couponRepository.SearchById(search, page, pageSize);
                var Count = TypeList.Count();
                return Ok(new { StatusCode = 200, Message = "Load successful", data = TypeList, Count });

            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }


        }
        [HttpGet("{id}")]
        public async Task<ActionResult> GetCouponById(string id)
        {
            try
            {
                var Result = await couponRepository.GetCouponById(id);
                return Ok(new { StatusCode = 200, Message = "Load successful", data = Result });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Create(Coupon coupon)
        {

            try
            {
                var newCoupon = new Coupon
                {
                    Active = coupon.Active,
                    Discount = coupon.Discount,
                    Id = coupon.Id,
                    Description = coupon.Description,
                    EndDate = coupon.EndDate,
                    StartDate = coupon.StartDate,
                    PriceRange = coupon.PriceRange
                };
                await couponRepository.AddCoupon(newCoupon);
                return Ok(new { StatusCode = 200, Message = "Add successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }


        }

        [HttpPut("{id}")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> update(string id, Coupon coupon)
        {
            if (id != coupon.Id)
            {
                return BadRequest();
            }
            try
            {
                var updateCoupon = new Coupon
                {
                    Active = coupon.Active,
                    Discount = coupon.Discount,
                    Id = coupon.Id,
                    Description = coupon.Description,
                    EndDate = coupon.EndDate,
                    StartDate = coupon.StartDate,
                    PriceRange = coupon.PriceRange
                };
                await couponRepository.UpdateCoupon(updateCoupon);
                return Ok(new { StatusCode = 200, Message = "Update successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }


        }

        [HttpPut("UpdateActive")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> UpdateActive(string id)
        {

            try
            {
                if (id != null)
                {
                    return Ok(new { StatusCode = 400, Message = "Id is not Exits" });
                }
                else
                {
                    Coupon coupon = await couponRepository.GetCouponById(id);
                    if (coupon == null)
                    {
                        return Ok(new { StatusCode = 400, Message = "Id not Exists" });

                    }
                    else
                    {
                        await couponRepository.UpdateActive(id, coupon.Active);
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
        public async Task<IActionResult> Delete(string id)
        {

            try
            {

                await couponRepository.DeleteCoupon(id);

                return Ok(new { StatusCode = 200, Message = "Delete successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }


        }
    }
}
