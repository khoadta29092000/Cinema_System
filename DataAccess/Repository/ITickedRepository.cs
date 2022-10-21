using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface ITickedRepository
    {
        Task<List<Ticked>> GetTickeds();

        Task<Ticked> GetTickedById(int SeatId, int BillId, int SchedulingId);

        Task DeleteTicked(int SeatId, int BillId, int SchedulingId);

        Task UpdateTicked(Ticked m);
        Task AddTicked(Ticked m);
        Task<List<Ticked>> FilterTicked(int SeatId, int BillId, int SchedulingId, int page, int pageSize);
    }
}
