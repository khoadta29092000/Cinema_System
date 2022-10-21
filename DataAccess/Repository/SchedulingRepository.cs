using BusinessObject.Models;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class SchedulingRepository : ISchedulingRepository
    {
        public Task<List<Scheduling>> GetSchedulings() => SchedulingDAO.GetSchedulings();

        public Task<Scheduling> GetSchedulingById(int SchedulingId) => SchedulingDAO.Instance.GetSchedulingById(SchedulingId);
        public Task DeleteScheduling(int m) => SchedulingDAO.DeleteScheduling(m);

        public Task UpdateActive(int SchedulingId, bool? active) => SchedulingDAO.Instance.UpdateActive(SchedulingId, active);
        public Task AddScheduling(Scheduling m) => SchedulingDAO.AddScheduling(m);
        public Task UpdateScheduling(Scheduling m) => SchedulingDAO.UpdateScheduling(m);
        public Task<List<Scheduling>> FilterScheduling(DateTime Date, int RoomId, int CinemaId, int FilmId, int page, int pageSize)
        => SchedulingDAO.Instance.FilterScheduling(Date, RoomId, CinemaId, FilmId, page, pageSize);
    }
}
