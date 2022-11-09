using Microsoft.AspNetCore.Mvc;
using BusinessObject.Models;
using DataAccess.Repository;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using CinemaSystem.Models;
using MoMo;
using System.Net.Mail;
using System.Net;
using System.IO;
using System.Web;
using MimeKit;

namespace CinemaSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillController : ControllerBase
    {
        private readonly IBillRepository billRepository;
        private readonly ITickedRepository tickedRepository;
        private readonly IServiceInBillRepository serviceInBillRepository;
        private readonly IServiceInCinemaRepository serviceInCinemaRepository;
        private readonly IServiceRepository serviceRepository;
        private readonly ISchedulingRepository schedulingRepository;
        private readonly ICouponRepository couponRepository;
        private readonly IConfiguration configuration;
        public BillController(ICouponRepository _couponRepository, ISchedulingRepository _schedulingRepository, IServiceInCinemaRepository _serviceInCinemaRepository, IServiceRepository _serviceRepository, IBillRepository _billRepository, IConfiguration configuration, ITickedRepository _tickedRepository, IServiceInBillRepository _serviceInBillRepository)
        {
            couponRepository = _couponRepository;
            schedulingRepository = _schedulingRepository;
            billRepository = _billRepository;
            this.configuration = configuration;
            tickedRepository = _tickedRepository;
            serviceInBillRepository = _serviceInBillRepository;
            serviceRepository = _serviceRepository;
            serviceInCinemaRepository = _serviceInCinemaRepository;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll( int AccountId, int PaymentId, string CouponId, int page, int pageSize)
        {

            try
            {
                var TypeList = await billRepository.FilterBill(AccountId, PaymentId, CouponId, page, pageSize);
                var Count = TypeList.Count();
                return Ok(new { StatusCode = 200, Message = "Load successful", data = TypeList, Count });

            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }


        }

        [HttpGet("GetPaymentLink/{amount}")]
        public async Task<IActionResult> GetPaymentLink(int amount)
        {

            try
            {
                string linkMoMoPayment = PaymentRequest.GetPaymentURL(amount.ToString());
                return Ok(new { StatusCode = 200, Message = "Load successful", data = linkMoMoPayment });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }

        }


        [HttpGet("{id}")]
        public async Task<ActionResult> GetBillById(int id)
        {
            try
            {
                var Result = await billRepository.GetBillById(id);
                return Ok(new { StatusCode = 200, Message = "Load successful", data = Result });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }
        }

        [HttpPost]
       //[Authorize(Roles = "1")]
        public async Task<IActionResult> Create(BillObject billObject)
        {

            try
            {
              

                Scheduling schedulingId = await schedulingRepository.GetSchedulingById(billObject.SchedulingId ?? default(int));
                var ServiceInCinemaList = await serviceInCinemaRepository.SearchByCinemaId(schedulingId.CinemaId ?? default(int), 100, 0);
                foreach (ServiceInBillObject item in billObject.ServiceInBillObject)
                {
                    if (ServiceInCinemaList.SingleOrDefault(x => x.ServiceId == item.ServiceId) == null)
                    {
                        return StatusCode(409, new { StatusCode = 409, Message = "Service not In cinema" });
                    }
                }

                DateTime toDay = DateTime.Now;
                var couponlist = await couponRepository.GetCoupons();
              
                if(billObject.CouponId != null)
                {
                     var couponid = await couponRepository.GetCouponById(billObject.CouponId);
                    if (couponlist.SingleOrDefault(x => x.Id == billObject.CouponId) == null )
                    {
                        return StatusCode(409, new { StatusCode = 409, Message = "Coupon Code not Exist" });
                    }
                    if (couponid.StartDate > toDay.Date || toDay.Date > couponid.EndDate )
                    {
                        return StatusCode(409, new { StatusCode = 409, Message = "Coupon out of date" });
                    }
                }

                int? allTotalBill = 0;
                int? quantity = 0;
                int? priceServiceAll = 0;
                int? priceSeatAll = 0;
                
         var newBill = new Bill
                    {
                        AccountId = billObject.AccountId,
                        PaymentId = billObject.PaymentId,
                        CouponId = billObject.CouponId,
                        SchedulingId = billObject.SchedulingId,
                        Time = DateTime.Now,
                        Total = billObject.Total,
                    };
                    await billRepository.AddBill(newBill);
                    var BillList = await billRepository.GetBills();


                    Bill idBill = BillList.LastOrDefault(x => x.Id != null);
                    foreach (ServiceInBillObject item in billObject.ServiceInBillObject)
                    {
                        var ServiceInCinemaId = ServiceInCinemaList.FirstOrDefault(x => x.ServiceId == item.ServiceId);

                        var newServiceInBill = new ServiceInBill
                        {
                            Checking = false,
                            BillId = idBill.Id,
                            ServiceInCinemaId = ServiceInCinemaId.Id,
                            Quantity = item.Quantity
                        };
                        await serviceInBillRepository.AddServiceInBill(newServiceInBill);

                        var priceService = await serviceRepository.GetServiceById(item.ServiceId);
                        priceServiceAll += priceService.Price * item.Quantity;
                        
                     
                        quantity = ServiceInCinemaId.Quantity - item.Quantity;
                        await serviceInCinemaRepository.UpdateQuantity(ServiceInCinemaId.Id, quantity ?? default(int));

                    }
                    foreach (TickedObject item in billObject.TickedObject)
                    {
                    var newTicked = new Ticked
                    {
                        BillId = idBill.Id,
                        SchedulingId = billObject.SchedulingId ?? default(int),
                        SeatId = item.SeatId,
                        AccountId = item.AccountId,
                        Price = 80,
                        Checking = false,
                        };
                        await tickedRepository.AddTicked(newTicked);
                         priceSeatAll += 80;
                      
                    }
                    if(billObject.CouponId != null)
                    {
                        var couponid = await couponRepository.GetCouponById(billObject.CouponId);
                       allTotalBill = (priceSeatAll + priceServiceAll) -(((priceSeatAll + priceServiceAll) * couponid.Discount)/100);
                        await billRepository.UpdateTotal(idBill.Id, allTotalBill ?? default(int));
                    }
                    else
                   {
                    allTotalBill = priceSeatAll + priceServiceAll;
                    await billRepository.UpdateTotal(idBill.Id, allTotalBill ?? default(int));
                }
                    
                   
                   
            
                return Ok(new { StatusCode = 200, Message = "Add successful" });


            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }


        }

        [HttpPost("SendMail")]
        //[Authorize(Roles = "1")]
        public async Task<IActionResult> SendMail(SendEmail sendEmail)
        {

            try {
                string ticked = string.Join(",", sendEmail.ticked.Select(p => p.ToString()).ToArray());
                string service = string.Join(",", sendEmail.service.Select(p => p.ToString()).ToArray());
                string FilePath = Directory.GetCurrentDirectory() + "\\HTML\\htm.html";
                StreamReader str = new StreamReader(FilePath);
                string MailText = str.ReadToEnd();
                str.Close();
                MailText = MailText.Replace("[film]", sendEmail.film).Replace("[time]", sendEmail.time).Replace("[cinema]", sendEmail.cinema).Replace("[room]", sendEmail.room)
                    .Replace("[date]", sendEmail.date).Replace("[startTime]", sendEmail.startTime).Replace("[billId]", sendEmail.billId).Replace("[image]", sendEmail.image)
                    .Replace("[service]", service).Replace("[ticked]", ticked);
                string subject = sendEmail.subject;
                string to = sendEmail.to;
                MailMessage mm = new MailMessage();
                mm.From = new MailAddress("tiensidien1234@gmail.com");
                mm.To.Add(to);
                mm.Subject = subject;
                mm.Body = MailText.ToString();
                mm.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient("smtp.gmail.com");
             
                smtp.Port = 587;
                            
                NetworkCredential nc = new NetworkCredential("tiensidien1234@gmail.com", "ooknrpifyewnklrf");
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = nc;
                smtp.Send(mm);
                return Ok(new { StatusCode = 200, Message = "send mail successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }


        }

      

        [HttpPut("{id}")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> update(int id, Scheduling scheduling)
        {
            if (id != scheduling.Id)
            {
                return BadRequest();
            }
            try
            {

              
                return Ok(new { StatusCode = 200, Message = "Update successful" });
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

                await billRepository.DeleteBill(id);

                return Ok(new { StatusCode = 200, Message = "Delete successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }


        }
        [HttpPut("UpdateActive")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> updatetotal(int id, int total)
        {

            try
            {

                await billRepository.UpdateTotal(id, total);

                return Ok(new { StatusCode = 200, Message = "Delete successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }


        }
    }
}
