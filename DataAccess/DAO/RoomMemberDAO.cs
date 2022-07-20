using BusinessObject.BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class RoomMemberDAO
    {
        private static RoomMemberDAO instance = null;
        private static readonly object instanceLock = new object();
        public static RoomMemberDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new RoomMemberDAO();
                    }
                    return instance;
                }
            }
        }
        public async Task AddRoomMember(RoomMember roomMember)
        {
            try
            {
                var HostelManagementContext = new HostelManagementContext();
                HostelManagementContext.Attach(roomMember).State = EntityState.Added;
                await HostelManagementContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<RoomMember>> GetRoomMemberList()
        {
            try
            {
                var HostelManagementContext = new HostelManagementContext();
                return await HostelManagementContext.RoomMembers
                    .Include(r => r.Rent)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateRoomMember(RoomMember roomMember)
        {
            try
            {
                var HostelManagementContext = new HostelManagementContext();
                HostelManagementContext.Attach(roomMember).State = EntityState.Modified;
                await HostelManagementContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<RoomMember> GetRoomMemberByID(int id)
        {
            try
            {
                var HostelManagementContext = new HostelManagementContext();
                return await HostelManagementContext.RoomMembers
                    .Include(r => r.Rent)
                    .FirstOrDefaultAsync(m => m.RoomMemberId == id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RoomMember> GetRoomMemberByEmail(string email, int rentId)
        {
            try
            {
                var HostelManagementContext = new HostelManagementContext();
                return await HostelManagementContext.RoomMembers
                    .Include(r => r.Rent)
                    .FirstOrDefaultAsync(m => m.UserEmail == email && m.RentId == rentId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
