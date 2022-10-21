using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DAO
{
    public class BillDAO
    {
        private static BillDAO instance = null;
        private static readonly object instanceLock = new object();
        private BillDAO() { }
        public static BillDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new BillDAO();
                    }
                    return instance;
                }
            }
        }
        public static async Task<List<Bill>> GetBills()
        {
            var bills = new List<Bill>();

            try
            {
                using (var context = new CinemaManagementContext())
                {
                    bills = await context.Bills.ToListAsync();

                }
                return bills;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        public static async Task AddBill(Bill m)
        {
            try
            {
                using (var context = new CinemaManagementContext())
                {               
                    var p2 = await context.Schedulings.FirstOrDefaultAsync(c => c.Id.Equals(m.Id));
                   
                        if (p2 == null)
                        {
                            context.Bills.Add(m);
                            await context.SaveChangesAsync();
                        }
                        else
                        {
                            throw new Exception("Id is Exits");
                        }
                    }                                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task UpdateBill(Bill m)
        {
            try
            {
                using (var context = new CinemaManagementContext())
                {


                    context.Entry<Bill>(m).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    await context.SaveChangesAsync();

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task DeleteBill(int p)
        {
            try
            {
                using (var context = new CinemaManagementContext())
                {
                    var bill = await context.Bills.FirstOrDefaultAsync(c => c.Id == p);
                    if (bill == null)
                    {
                        throw new Exception("Id is not Exits");
                    }
                    else
                    {
                        context.Bills.Remove(bill);
                        await context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public async Task<List<Bill>> FilterBill( int AccountId, int PaymentId, string CouponId, int page, int pageSize)
        {
            try
            {
                List<Bill> searchResult = null;
                IEnumerable<Bill> searchValues = await GetBills();
                if (page == 0 || pageSize == 0)
                {
                    page = 1;
                    pageSize = 1000;
                }         
                if (AccountId != 0)
                {
                    searchValues = searchValues.Where(c => c.AccountId == AccountId);
                }
                if (PaymentId != 0)
                {
                    searchValues = searchValues.Where(c => c.PaymentId == PaymentId);
                }
                if (!string.IsNullOrEmpty(CouponId))
                {
                    searchValues = searchValues.Where(c => c.CouponId == CouponId);
                }
                searchValues = searchValues.Skip((page - 1) * pageSize).Take(pageSize);
                searchResult = searchValues.ToList();
                return searchResult;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }
        public async Task<Bill> GetBillById(int BillId)
        {

            IEnumerable<Bill> bills = await GetBills();
            Bill bill = bills.SingleOrDefault(mb => mb.Id == BillId);
            return bill;
        }

        public async Task UpdateTotal(int BillId, int total)
        {

            try
            {

                var bill = new Bill() { Id = BillId, Total = total };
                using (var db = new CinemaManagementContext())
                {
                    db.Bills.Attach(bill);
                    db.Entry(bill).Property(x => x.Total).IsModified = true;
                    await db.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
