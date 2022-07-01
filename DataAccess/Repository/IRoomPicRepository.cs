using BusinessObject.BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IRoomPicRepository
    {
        Task AddRoomPic(RoomPic RoomPic);
        Task<IEnumerable<RoomPic>> GetRoomPicsOfARoom(int RoomId);
        Task DeleteRoomPic(RoomPic RoomPic);
    }
}
