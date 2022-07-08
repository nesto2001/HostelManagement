using BusinessObject.BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IRoomMemberRepository
    {
        Task AddRoomMember(RoomMember RoomMember); 
        Task<IEnumerable<RoomMember>> GetRoomMemberList();
        Task UpdateRoomMember(RoomMember Room); 
        Task<RoomMember> GetRoomMemberByID(int id);
    }
}
