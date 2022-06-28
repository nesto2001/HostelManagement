using BusinessObject.BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IRoomRepository
    {
        Task<Room> GetRoomByID(int id);
        Task UpdateRoom(Room Room);
        Task DeleteRoom(Room Room);
        Task AddRoom(Room Room);
        Task<IEnumerable<Room>> GetRoomList();
        Task<IEnumerable<Room>> GetRoomsOfAHostel(int hostelId);
    }
}
