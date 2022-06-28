using BusinessObject.BusinessObject;
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
        public async Task AddRoom(Room Room) => await RoomDAO.Instance.AddRoom(Room);
        public async Task DeleteRoom(Room Room) => await RoomDAO.Instance.DeleteRoom(Room);

        public async Task<Room> GetRoomByID(int id) => await RoomDAO.Instance.GetRoomByID(id);

        public async Task<IEnumerable<Room>> GetRoomList() => await RoomDAO.Instance.GetRoomsList();
        public async Task<IEnumerable<Room>> GetRoomsOfAHostel(int hostelId) => await RoomDAO.Instance.GetRoomsOfAHostel(hostelId);

        public async Task UpdateRoom(Room Room) => await RoomDAO.Instance.UpdateRoom(Room);
    }
}
