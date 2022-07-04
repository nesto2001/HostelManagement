using BusinessObject.BusinessObject;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class RoomMemberRepository : IRoomMemberRepository
    {
        public async Task AddRoomMember(RoomMember RoomMember) => await RoomMemberDAO.Instance.AddRoomMember(RoomMember);
    }
}
