using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IRoomRepository
    {
        Task<List<RoomDTO>> GetRooms();

        Task<RoomDTO> GetRoomById(int RoomId);
        Task UpdateActive(int RoomId, bool? active);
        Task DeleteRoom(int m);

        Task UpdateRoom(Room m);
        Task AddRoom(Room m);
        Task<List<RoomDTO>> SearchByTitle(string search,int RoleId, int page, int pageSize);
    }
}
