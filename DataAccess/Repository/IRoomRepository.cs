using BusinessObject.BusinessObject;
using System.Collections.Generic;
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
        Task ActivateRoom(int id);
        Task DenyRoom(int id);
        Task PendingRoom(int id);
    }
}
