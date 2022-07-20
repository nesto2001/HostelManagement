using BusinessObject.BusinessObject;
using DataAccess.DAO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class RoomRepository : IRoomRepository
    {
        public async Task AddRoom(Room Room) => await RoomDAO.Instance.AddRoom(Room);
        public async Task DeleteRoom(Room Room) => await RoomDAO.Instance.DeleteRoom(Room);

        public async Task<Room> GetRoomByID(int id) => await RoomDAO.Instance.GetRoomByID(id);

        public async Task<IEnumerable<Room>> GetRoomList() => await RoomDAO.Instance.GetRoomsList();
        public async Task<IEnumerable<Room>> GetRoomsOfAHostel(int hostelId) => await RoomDAO.Instance.GetRoomsOfAHostel(hostelId);

        public async Task UpdateRoom(Room Room) => await RoomDAO.Instance.UpdateRoom(Room);
        public async Task ActivateRoom(int id) => await RoomDAO.Instance.ActivateRoom(id);
        public async Task DenyRoom(int id) => await RoomDAO.Instance.DenyRoom(id);
        public async Task PendingRoom(int id) => await RoomDAO.Instance.PendingRoom(id);
    }
}
