using BusinessObject.Models;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class ServiceInBillRepository : IServiceInBillRepository
    {
        public Task<List<ServiceInBill>> GetServiceInBills() => ServiceInBillDAO.GetServiceInBills();
        public Task<List<ServiceDTO>> GetServiceInBills1() => ServiceInBillDAO.GetServiceInBills1();
        public Task<ServiceInBill> GetServiceInBillById(int ServiceInCinemaId, int BillId) => ServiceInBillDAO.Instance.GetServiceInBillById(ServiceInCinemaId, BillId);
        public Task DeleteServiceInBill(int ServiceInCinemaId, int BillId) => ServiceInBillDAO.DeleteServiceInBill(ServiceInCinemaId, BillId);
        public Task AddServiceInBill(ServiceInBill m) => ServiceInBillDAO.AddServiceInBill(m);
        public Task UpdateServiceInBill(ServiceInBill m) => ServiceInBillDAO.UpdateServiceInBill(m);
        public Task<List<ServiceInBill>> SearchByCinemaId(int ServiceId, int page, int pageSize)
        => ServiceInBillDAO.Instance.SearchByCinemaId(ServiceId, page, pageSize);
    }
}
