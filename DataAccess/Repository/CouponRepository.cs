using BusinessObject.Models;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class CouponRepository : ICouponRepository
    {
        public Task<List<Coupon>> GetCoupons() => CouponDAO.GetCoupons();
        public Task<Coupon> GetCouponById(string CouponId) => CouponDAO.Instance.GetCouponById(CouponId);
        public Task DeleteCoupon(string m) => CouponDAO.DeleteCoupon(m);
        public Task UpdateActive(string CouponId, bool? active) => CouponDAO.Instance.UpdateActive(CouponId, active);
        public Task AddCoupon(Coupon m) => CouponDAO.AddCoupon(m);
        public Task UpdateCoupon(Coupon m) => CouponDAO.UpdateCoupon(m);
        public Task<List<Coupon>> SearchById(string search, int page, int pageSize) => CouponDAO.Instance.SearchById(search, page, pageSize);
    }
}
