using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface ISeatRepository
    {
        Task<List<Seat>> GetSeats();

        Task<Seat> GetSeatById(int SeatId);
        Task UpdateActive(int SeatId, bool? active);
        Task DeleteSeat(int m);

        Task UpdateSeat(Seat m);
        Task AddSeat(Seat m);
        Task<List<Seat>> SearchByTitle(string search, int page, int pageSize);
    }
}
