using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DAO
{
    public class CouponDAO
    {
        private static CouponDAO instance = null;
        private static readonly object instanceLock = new object();
        private CouponDAO() { }
        public static CouponDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new CouponDAO();
                    }
                    return instance;
                }
            }
        }
        public static async Task<List<Coupon>> GetCoupons()
        {
            var coupons = new List<Coupon>();

            try
            {
                using (var context = new CinemaManagementContext())
                {
                    coupons = await context.Coupons.ToListAsync();

                }
                return coupons;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        public static async Task AddCoupon(Coupon m)
        {
            try
            {


                using (var context = new CinemaManagementContext())
                {
                   
                    var p2 = await context.Coupons.FirstOrDefaultAsync(c => c.Id.Equals(m.Id));
                   
                        if (p2 == null)
                        {
                            context.Coupons.Add(m);
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

        public static async Task UpdateCoupon(Coupon m)
        {
            try
            {
                using (var context = new CinemaManagementContext())
                {

                    var p1 = await context.Coupons.FirstOrDefaultAsync(c => c.Id.Equals(m.Id));

                    if (p1 == null)
                    {

                        context.Entry<Coupon>(m).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        await context.SaveChangesAsync();

                    }
                    else
                    {
                        if (p1.Id == m.Id)
                        {
                            context.Entry<Coupon>(m).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                            await context.SaveChangesAsync();
                        }

                        throw new Exception("Coupon Code is Exits");
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task DeleteCoupon(string p)
        {
            try
            {
                using (var context = new CinemaManagementContext())
                {
                    var coupon = await context.Coupons.FirstOrDefaultAsync(c => c.Id == p);
                    if (coupon == null)
                    {
                        throw new Exception("Id is not Exits");
                    }
                    else
                    {
                        context.Coupons.Remove(coupon);
                        await context.SaveChangesAsync();
                    }



                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public async Task<List<Coupon>> SearchById(string search, int page, int pageSize)
        {
            List<Coupon> searchResult = null;
            if (page == 0 || pageSize == 0)
            {
                page = 1;
                pageSize = 1000;
            }
            if (search == null)
            {
                IEnumerable<Coupon> searchValues = await GetCoupons();
                searchValues = searchValues.Skip((page - 1) * pageSize).Take(pageSize);
                searchResult = searchValues.ToList();
            }
            else
            {
                using (var context = new CinemaManagementContext())
                {
                    IEnumerable<Coupon> searchValues = await (from Coupon in context.Coupons
                                                              where Coupon.Id.ToLower().Contains(search.ToLower())
                                                             select Coupon).ToListAsync();
                    searchValues = searchValues.Skip((page - 1) * pageSize).Take(pageSize);
                    searchResult = searchValues.ToList();
                }
            }

            return searchResult;
        }
        public async Task<Coupon> GetCouponById(string CouponId)
        {

            IEnumerable<Coupon> coupons = await GetCoupons();
            Coupon coupon = coupons.SingleOrDefault(mb => mb.Id.ToLower().Contains(CouponId.ToLower()));
            return coupon;
        }
        public async Task UpdateActive(string CouponId, bool? acticve)
        {

            try
            {

                var coupon = new Coupon() { Id = CouponId, Active = !acticve };
                using (var db = new CinemaManagementContext())
                {
                    db.Coupons.Attach(coupon);
                    db.Entry(coupon).Property(x => x.Active).IsModified = true;
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
