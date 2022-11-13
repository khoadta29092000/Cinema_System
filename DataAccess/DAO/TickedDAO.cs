using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DAO
{
    public class TickedDAO
    {
        private static TickedDAO instance = null;
        private static readonly object instanceLock = new object();
        private TickedDAO() { }
        public static TickedDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new TickedDAO();
                    }
                    return instance;
                }
            }
        }
        public static async Task<List<Ticked>> GetTickeds()
        {
            var bills = new List<Ticked>();

            try
            {
                using (var context = new CinemaManagementContext())
                {
                    bills = await context.Tickeds.ToListAsync();

                }
                return bills;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        public static async Task AddTicked(Ticked m)
        {
            try
            {
                using (var context = new CinemaManagementContext())
                {
                    var p2 = await context.Tickeds.FirstOrDefaultAsync(c => c.SeatId.Equals(m.SeatId) && c.BillId == m.BillId && c.SchedulingId == m.SchedulingId);

                    if (p2 == null)
                    {
                        context.Tickeds.Add(m);
                        await context.SaveChangesAsync();
                    }
                    else
                    {
                        throw new Exception("Id is Exits");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task UpdateTicked(Ticked m)
        {
            try
            {
                using (var context = new CinemaManagementContext())
                {


                    context.Entry<Ticked>(m).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    await context.SaveChangesAsync();

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task DeleteTicked(int SeatId, int BillId, int SchedulingId)
        {
            try
            {
                using (var context = new CinemaManagementContext())
                {
                    var ticked = await context.Tickeds.FirstOrDefaultAsync(c => c.SeatId == SeatId && c.BillId == BillId && c.SchedulingId == SchedulingId);
                    if (ticked == null)
                    {
                        throw new Exception("Id is not Exits");
                    }
                    else
                    {
                        context.Tickeds.Remove(ticked);
                        await context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public async Task<List<Ticked>> FilterTicked(int SeatId, int BillId, int SchedulingId, int page, int pageSize)
        {
            try
            {
                List<Ticked> searchResult = null;
                IEnumerable<Ticked> searchValues = await GetTickeds();
                if (page == 0 || pageSize == 0)
                {
                    page = 1;
                    pageSize = 1000;
                }
                if (SeatId != 0)
                {
                    searchValues = searchValues.Where(c => c.SeatId == SeatId);
                }
                if (BillId != 0)
                {
                    searchValues = searchValues.Where(c => c.BillId == BillId);
                }
                if (SchedulingId != 0)
                {
                    searchValues = searchValues.Where(c => c.SchedulingId == SchedulingId);
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
        public async Task<Ticked> GetTickedById(int SeatId, int BillId, int SchedulingId)
        {

            IEnumerable<Ticked> tickeds = await GetTickeds();
            Ticked ticked = tickeds.SingleOrDefault(c => c.SeatId.Equals(SeatId) && c.BillId == BillId && c.SchedulingId == SchedulingId);
            return ticked;
        }
        public async Task UpdateChecking(int SeatId, int BillId, int SchedulingId, int AccountId, bool? checking)
        {

            try
            {

                var type = new Ticked() { SchedulingId = SchedulingId, SeatId = SeatId, BillId = BillId, AccountId = AccountId, Checking = true };
                using (var db = new CinemaManagementContext())
                {
                    db.Tickeds.Attach(type);
                    db.Entry(type).Property(x => x.Checking).IsModified = true;
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
