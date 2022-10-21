using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface ICouponRepository
    {
        Task<List<Coupon>> GetCoupons();

        Task<Coupon> GetCouponById(string CouponId);
        Task UpdateActive(string CouponId, bool? active);
        Task DeleteCoupon(string m);

        Task UpdateCoupon(Coupon m);
        Task AddCoupon(Coupon m);
        Task<List<Coupon>> SearchById(string search, int page, int pageSize);
    }
}
