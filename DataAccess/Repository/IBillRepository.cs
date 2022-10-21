using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IBillRepository
    {
        Task<List<Bill>> GetBills();

        Task<Bill> GetBillById(int BillId);
     
        Task DeleteBill(int m);

        Task UpdateBill(Bill m);
        Task AddBill(Bill m);

        Task UpdateTotal(int BillId, int total);
        Task<List<Bill>> FilterBill(int AccountId, int PaymentId, string CouponId, int page, int pageSize);
    }
}
