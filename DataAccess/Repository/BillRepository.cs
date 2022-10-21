using BusinessObject.Models;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class BillRepository : IBillRepository
    {
        public Task<List<Bill>> GetBills() => BillDAO.GetBills();

        public Task<Bill> GetBillById(int BillId) => BillDAO.Instance.GetBillById(BillId);
        public Task DeleteBill(int m) => BillDAO.DeleteBill(m);
        public Task AddBill(Bill m) => BillDAO.AddBill(m);
        public Task UpdateBill(Bill m) => BillDAO.UpdateBill(m);
        public Task UpdateTotal(int BillId, int Total) => BillDAO.Instance.UpdateTotal(BillId, Total);
        public Task<List<Bill>> FilterBill(int AccountId, int PaymentId, string CouponId, int page, int pageSize)
        => BillDAO.Instance.FilterBill(AccountId, PaymentId, CouponId, page, pageSize);
    }
}
