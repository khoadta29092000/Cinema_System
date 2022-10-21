using BusinessObject.Models;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class RoomRepository : IRoomRepository
    {
        public Task<List<Room>> GetRooms() => RoomDAO.GetRooms();

        public Task<Room> GetRoomById(int RoomId) => RoomDAO.Instance.GetRoomById(RoomId);
        public Task DeleteRoom(int m) => RoomDAO.DeleteRoom(m);

        public Task UpdateActive(int RoomId, bool? active) => RoomDAO.Instance.UpdateActive(RoomId, active);
        public Task AddRoom(Room m) => RoomDAO.AddRoom(m);
        public Task UpdateRoom(Room m) => RoomDAO.UpdateRoom(m);
        public Task<List<Room>> SearchByTitle(string search, int page, int pageSize) => RoomDAO.Instance.SearchByTitle(search, page, pageSize);
    }
}
