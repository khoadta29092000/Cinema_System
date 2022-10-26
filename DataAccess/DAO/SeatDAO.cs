using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DAO
{
    public class SeatDAO
    {
        private static SeatDAO instance = null;
        private static readonly object instanceLock = new object();
        private SeatDAO() { }
        public static SeatDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new SeatDAO();
                    }
                    return instance;
                }
            }
        }
        public static async Task<List<Seat>> GetSeats()
        {
            var seats = new List<Seat>();

            try
            {
                using (var context = new CinemaManagementContext())
                {
                    seats = await context.Seats.ToListAsync();

                }
                return seats;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        public static async Task AddSeat(Seat m)
        {
            try
            {
                using (var context = new CinemaManagementContext())
                {
                    var p1 = await context.Seats.FirstOrDefaultAsync(c => c.Title.Equals(m.Title) && c.RoomId == m.RoomId);
                    var p2 = await context.Seats.FirstOrDefaultAsync(c => c.Id.Equals(m.Id));
                    if (p1 == null)
                    {
                        if (p2 == null)
                        {
                            context.Seats.Add(m);
                            await context.SaveChangesAsync();
                        }
                        else
                        {
                            throw new Exception("Id is Exits");
                        }
                    }
                    else
                    {
                        throw new Exception("Title is Exist");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task UpdateSeat(Seat m)
        {
            try
            {
                using (var context = new CinemaManagementContext())
                {
                    var p1 = await context.Seats.FirstOrDefaultAsync(c => c.Title.Equals(m.Title));

                    if (p1 == null)
                    {

                        context.Entry<Seat>(m).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        await context.SaveChangesAsync();

                    }
                    else
                    {
                        if (p1.Title == m.Title)
                        {
                            context.Entry<Seat>(m).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                            await context.SaveChangesAsync();
                        }

                        throw new Exception("Title is Exits");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task DeleteSeat(int p)
        {
            try
            {
                using (var context = new CinemaManagementContext())
                {
                    var seat = await context.Seats.FirstOrDefaultAsync(c => c.Id == p);
                    if (seat == null)
                    {
                        throw new Exception("Id is not Exits");
                    }
                    else
                    {
                        context.Seats.Remove(seat);
                        await context.SaveChangesAsync();
                    }



                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public async Task<List<Seat>> SearchByTitle(string search, int page, int pageSize)
        {
            List<Seat> searchResult = null;
            if (page == 0 || pageSize == 0)
            {
                page = 1;
                pageSize = 1000;
            }
            if (search == null)
            {
                IEnumerable<Seat> searchValues = await GetSeats();
                searchValues = searchValues.Skip((page - 1) * pageSize).Take(pageSize);
                searchResult = searchValues.ToList();
            }
            else
            {
                using (var context = new CinemaManagementContext())
                {
                    IEnumerable<Seat> searchValues = await (from seat in context.Seats
                                                            where seat.Title.ToLower().Contains(search.ToLower())
                                                            select seat).ToListAsync();
                    searchValues = searchValues.Skip((page - 1) * pageSize).Take(pageSize);
                    searchResult = searchValues.ToList();
                }
            }

            return searchResult;
        }
        public async Task<Seat> GetSeatById(int RoomId)
        {

            IEnumerable<Seat> seats = await GetSeats();
            Seat seat = seats.SingleOrDefault(mb => mb.Id == RoomId);
            return seat;
        }
        public async Task UpdateActive(int SeatId, bool? acticve)
        {

            try
            {

                var seat = new Seat() { Id = SeatId, Active = !acticve };
                using (var db = new CinemaManagementContext())
                {
                    db.Seats.Attach(seat);
                    db.Entry(seat).Property(x => x.Active).IsModified = true;
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
