using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DAO
{
    public class RoomDAO
    {
        private static RoomDAO instance = null;
        private static readonly object instanceLock = new object();
        private RoomDAO() { }
        public static RoomDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new RoomDAO();
                    }
                    return instance;
                }
            }
        }
        public static async Task<List<Room>> GetRooms()
        {
            var rooms = new List<Room>();

            try
            {
                using (var context = new CinemaManagementContext())
                {
                    rooms = await context.Rooms.ToListAsync();

                }
                return rooms;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        public static async Task AddRoom(Room m)
        {
            try
            {
                using (var context = new CinemaManagementContext())
                {
                    var p1 = await context.Rooms.FirstOrDefaultAsync(c => c.Title.Equals(m.Title) && c.CinemaId == m.CinemaId);
                    var p2 = await context.Rooms.FirstOrDefaultAsync(c => c.Id.Equals(m.Id));
                    if (p1 == null)
                    {
                        if (p2 == null)
                        {
                            context.Rooms.Add(m);
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

        public static async Task UpdateRoom(Room m)
        {
            try
            {
                using (var context = new CinemaManagementContext())
                {


                    context.Entry<Room>(m).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    await context.SaveChangesAsync();

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task DeleteRoom(int p)
        {
            try
            {
                using (var context = new CinemaManagementContext())
                {
                    var room = await context.Rooms.FirstOrDefaultAsync(c => c.Id == p);
                    if (room == null)
                    {
                        throw new Exception("Id is not Exits");
                    }
                    else
                    {
                        context.Rooms.Remove(room);
                        await context.SaveChangesAsync();
                    }



                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public async Task<List<Room>> SearchByTitle(string search, int page, int pageSize)
        {
            List<Room> searchResult = null;
            if (page == 0 || pageSize == 0)
            {
                page = 1;
                pageSize = 1000;
            }
            if (search == null)
            {
                IEnumerable<Room> searchValues = await GetRooms();
                searchValues = searchValues.Skip((page - 1) * pageSize).Take(pageSize);
                searchResult = searchValues.ToList();
            }
            else
            {
                using (var context = new CinemaManagementContext())
                {
                    IEnumerable<Room> searchValues = await (from room in context.Rooms
                                                            where room.Title.ToLower().Contains(search.ToLower())
                                                             select room).ToListAsync();
                    searchValues = searchValues.Skip((page - 1) * pageSize).Take(pageSize);
                    searchResult = searchValues.ToList();
                }
            }

            return searchResult;
        }
        public async Task<Room> GetRoomById(int RoomId)
        {

            IEnumerable<Room> rooms = await GetRooms();
            Room room = rooms.SingleOrDefault(mb => mb.Id == RoomId);
            return room;
        }
        public async Task UpdateActive(int RoomId, bool? acticve)
        {

            try
            {

                var room = new Room() { Id = RoomId, Active = !acticve };
                using (var db = new CinemaManagementContext())
                {
                    db.Rooms.Attach(room);
                    db.Entry(room).Property(x => x.Active).IsModified = true;
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
