using BusinessObject.Models;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class TickedRepository : ITickedRepository
    {
        public Task<List<Ticked>> GetTickeds() => TickedDAO.GetTickeds();

        public Task<Ticked> GetTickedById(int SeatId, int BillId, int SchedulingId) => TickedDAO.Instance.GetTickedById(SeatId,BillId,SchedulingId);
        public Task DeleteTicked(int SeatId, int BillId, int SchedulingId) => TickedDAO.DeleteTicked(SeatId, BillId, SchedulingId);
        public Task AddTicked(Ticked m) => TickedDAO.AddTicked(m);
        public Task UpdateTicked(Ticked m) => TickedDAO.UpdateTicked(m);
        public Task<List<Ticked>> FilterTicked(int SeatId, int BillId, int SchedulingId, int page, int pageSize)
        => TickedDAO.Instance.FilterTicked(SeatId, BillId, SchedulingId, page, pageSize);
    }
}
