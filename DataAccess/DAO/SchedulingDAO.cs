using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DAO
{
    public class SchedulingDAO
    {
        private static SchedulingDAO instance = null;
        private static readonly object instanceLock = new object();
        private SchedulingDAO() { }
        public static SchedulingDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new SchedulingDAO();
                    }
                    return instance;
                }
            }
        }
        public static async Task<List<Scheduling>> GetSchedulings()
        {
            var schedulings = new List<Scheduling>();

            try
            {
                using (var context = new CinemaManagementContext())
                {
                    schedulings = await context.Schedulings.ToListAsync();

                }
                return schedulings;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        public static async Task AddScheduling(Scheduling m)
        {
            try
            {
                using (var context = new CinemaManagementContext())
                {
                    var p1 = await context.Schedulings.FirstOrDefaultAsync(c => c.CinemaId == m.CinemaId && c.RoomId == m.RoomId && c.Date == m.Date
                    && ((c.StartTime >= m.StartTime || m.StartTime <= c.EndTime) || (c.StartTime >= m.EndTime || m.EndTime <= c.EndTime) ));
                    var p2 = await context.Schedulings.FirstOrDefaultAsync(c => c.Id.Equals(m.Id));
                    if (p1 == null)
                    {
                        if (p2 == null)
                        {
                            context.Schedulings.Add(m);
                            await context.SaveChangesAsync();
                        }
                        else
                        {
                            throw new Exception("Id is Exits");
                        }
                    }
                    else
                    {
                        throw new Exception("Scheduling is Exist in Cinema");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task UpdateScheduling(Scheduling m)
        {
            try
            {
                using (var context = new CinemaManagementContext())
                {
                    var p1 = await context.Schedulings.FirstOrDefaultAsync(c => c.CinemaId == m.CinemaId && c.RoomId == m.RoomId && c.Date == m.Date
                                        && ((c.StartTime >= m.StartTime || m.StartTime <= c.EndTime) || (c.StartTime >= m.EndTime || m.EndTime <= c.EndTime)));
                 
                    if (p1 == null)
                    {

                        context.Entry<Scheduling>(m).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        await context.SaveChangesAsync();

                    }
                    else
                    {
                        if (p1.StartTime == m.StartTime && p1.Id == m.Id)
                        {
                            context.Entry<Scheduling>(m).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                            await context.SaveChangesAsync();
                        }
                        throw new Exception("Scheduling is Exist in Cinema");
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task DeleteScheduling(int p)
        {
            try
            {
                using (var context = new CinemaManagementContext())
                {
                    var scheduling = await context.Schedulings.FirstOrDefaultAsync(c => c.Id == p);
                    if (scheduling == null)
                    {
                        throw new Exception("Id is not Exits");
                    }
                    else
                    {
                        context.Schedulings.Remove(scheduling);
                        await context.SaveChangesAsync();
                    }



                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public async Task<List<Scheduling>> FilterScheduling(DateTime Date,int RoomId,int CinemaId,int FilmId, int page, int pageSize)
        {
            try
            {
                DateTime date1 = new DateTime(0001, 1, 1, 0, 0, 0);
                DateTime date2 = Date;
                List<Scheduling> searchResult = null;
                IEnumerable<Scheduling> searchValues = await GetSchedulings();
                if (page == 0 || pageSize == 0)
                {
                    page = 1;
                    pageSize = 1000;
                }
                if (DateTime.Compare(date1, date2) != 0)
                {                
                    searchValues = searchValues.Where(c => c.Date == Date);
                }
                if (RoomId != 0)
                {
                    searchValues = searchValues.Where(c => c.RoomId == RoomId);
                }
                if (CinemaId != 0)
                {
                    searchValues = searchValues.Where(c => c.CinemaId == CinemaId);
                }
                if (FilmId != 0)
                {
                    searchValues = searchValues.Where(c => c.FilmId == FilmId);
                }
                searchValues = searchValues.Skip((page - 1) * pageSize).Take(pageSize);
                searchResult = searchValues.ToList();
                return searchResult;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }
        public async Task<Scheduling> GetSchedulingById(int SchedulingId)
        {

            IEnumerable<Scheduling> schedulings = await GetSchedulings();
            Scheduling scheduling = schedulings.SingleOrDefault(mb => mb.Id == SchedulingId);
            return scheduling;
        }
        public async Task UpdateActive(int SchedulingId, bool? acticve)
        {

            try
            {

                var scheduling = new Scheduling() { Id = SchedulingId, Active = !acticve };
                using (var db = new CinemaManagementContext())
                {
                    db.Schedulings.Attach(scheduling);
                    db.Entry(scheduling).Property(x => x.Active).IsModified = true;
                    await db.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
