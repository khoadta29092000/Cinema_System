using BusinessObject.Models;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class SeatRepository : ISeatRepository
    {
        public Task<List<Seat>> GetSeats() => SeatDAO.GetSeats();

        public Task<Seat> GetSeatById(int SeatId) => SeatDAO.Instance.GetSeatById(SeatId);
        public Task DeleteSeat(int m) => SeatDAO.DeleteSeat(m);

        public Task UpdateActive(int SeatId, bool? active) => SeatDAO.Instance.UpdateActive(SeatId, active);
        public Task AddSeat(Seat m) => SeatDAO.AddSeat(m);
        public Task UpdateSeat(Seat m) => SeatDAO.UpdateSeat(m);
        public Task<List<Seat>> SearchByTitle(string search, int page, int pageSize) => SeatDAO.Instance.SearchByTitle(search, page, pageSize);
    }
}
