using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface ISchedulingRepository
    {
        Task<List<Scheduling>> GetSchedulings();

        Task<Scheduling> GetSchedulingById(int SchedulingId);
        Task UpdateActive(int SchedulingId, bool? active);
        Task DeleteScheduling(int m);

        Task UpdateScheduling(Scheduling m);
        Task AddScheduling(Scheduling m);
        Task<List<Scheduling>> FilterScheduling(DateTime Date, int RoomId, int CinemaId, int FilmId, int page, int pageSize);
    }
}
