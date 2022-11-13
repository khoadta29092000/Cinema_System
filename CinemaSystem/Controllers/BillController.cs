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
using CinemaSystem.ViewModel;
using System.Web.Http.Results;
using System.Collections;
using System.Collections.Generic;

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

        [HttpGet("HistoryBill/{AccountId}")]
        public async Task<ActionResult> GetHistoryBillByAccountID(int AccountId)
        {
            try
            {
                using (var dbContext = new CinemaManagementContext())
                {
                    var listOfTicketsWithAccountId = dbContext.Tickeds.Where(tic => tic.AccountId == AccountId).ToList();
                    if (listOfTicketsWithAccountId.Any()) {

                        var listOfHistory = listOfTicketsWithAccountId.GroupBy(tic => tic.BillId).Select(x => x.FirstOrDefault()).ToList()
                            .Select(t => dbContext.Bills.Find(t.BillId)).ToList()
                            //.Where(ti => ti != null)
                            .Select(bill => new BillVM()
                            {
                                Id = bill.Id,
                                Checking = (bool)dbContext.Tickeds.Where(ticc => ticc.BillId == bill.Id).ToList().FirstOrDefault(x => true).Checking,
                                SchedulingId = bill.SchedulingId,
                                PaymentId = bill.PaymentId,
                                CouponId = bill.CouponId,
                                Time = bill.Time,
                                Total = bill.Total,
                            }).ToList();

                        return Ok(new { StatusCode = 200, Message = "Load successful", data = listOfHistory });
                     } else
                    {
                        return Ok(new { StatusCode = 200, Message = "No Ticket for AccountId", data = new List<BillVM>() });
                    }
                }
                
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }
        }

        [HttpGet("AllHistoryBill")]
        public async Task<ActionResult> GetAllHistoryBill()
        {
            try
            {
                using (var dbContext = new CinemaManagementContext())
                {
                    var listAccountIsCustomer = dbContext.Accounts.Where(acc => acc.RoleId == 3).Select(x => x.Id).ToList();

                    var listOfTicketsWithAccountId = dbContext.Tickeds.Where(tic => listAccountIsCustomer.Contains(tic.AccountId)).ToList();
                    if (listOfTicketsWithAccountId.Any())
                    {

                        var listOfHistory = listOfTicketsWithAccountId.GroupBy(tic => tic.BillId).Select(x => x.FirstOrDefault()).ToList()
                            .Select(t => dbContext.Bills.Find(t.BillId)).ToList()
                            //.Where(ti => ti != null)
                            .Select(bill => new BillVM()
                            {
                                Id = bill.Id,
                                Checking = (bool)dbContext.Tickeds.Where(ticc => ticc.BillId == bill.Id).ToList().FirstOrDefault(x => true).Checking,
                                SchedulingId = bill.SchedulingId,
                                PaymentId = bill.PaymentId,
                                CouponId = bill.CouponId,
                                Time = bill.Time,
                                Total = bill.Total,
                            }).ToList();

                        return Ok(new { StatusCode = 200, Message = "Load successful", data = listOfHistory });
                    }
                    else
                    {
                        return Ok(new { StatusCode = 200, Message = "No Ticket for AccountId", data = new List<BillVM>() });
                    }
                }

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
                var BillList = await billRepository.GetBills();
                Bill idBill = BillList.LastOrDefault(x => x.Id != null);
                
                string FilePath = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory) + "\\HTML\\htm.html";
                StreamReader str = new StreamReader(FilePath);
                string MailText = str.ReadToEnd();
                str.Close();
                MailText = MailText.Replace("[film]", sendEmail.film).Replace("[time]", sendEmail.time).Replace("[cinema]", sendEmail.cinema).Replace("[room]", sendEmail.room)
                    .Replace("[date]", sendEmail.date).Replace("[startTime]", sendEmail.startTime).Replace("[billId]", idBill.Id.ToString()).Replace("[image]", sendEmail.image)
                    .Replace("[service]", sendEmail.service).Replace("[ticked]", sendEmail.ticked).Replace("[Total]", sendEmail.Total);
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
        public async Task<IActionResult> update(int id, Bill scheduling)
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

        [HttpPut("Checking")]
        [Authorize(Roles = "2")]
        public async Task<IActionResult> updateChecking(int id, int accountId)
        {
   
           
            try
            {
                List<Ticked> ticked = await tickedRepository.FilterTicked(0, id, 0, 100, 0);
                List<ServiceInBill> service = await serviceInBillRepository.SearchByCinemaId(id, 100, 0);
                foreach (Ticked item in ticked)
                {
                   await tickedRepository.UpdateChecking(item.SeatId, item.BillId, item.SchedulingId, item.AccountId ,item.Checking);
                }
                foreach (ServiceInBill item in service)
                {
                    await serviceInBillRepository.UpdateChecking(item.ServiceInCinemaId, item.BillId, item.Checking);
                }
                await billRepository.UpdateEmloyeeChecking(id ,accountId);
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
        [HttpPut("UpdateTotal")]
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
