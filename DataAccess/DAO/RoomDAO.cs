using BusinessObject.BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class RoomDAO
    {
        private static RoomDAO instance = null;
        private static readonly object instanceLock = new object();
        public static RoomDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new RoomDAO();
                    }
                    return instance;
                }
            }
        }
        public async Task AddRoom(Room room)
        {
            try
            {
                var HostelManagementContext = new HostelManagementContext();
                HostelManagementContext.Attach(room).State = EntityState.Added;
                await HostelManagementContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteRoom(Room Room)
        {
            throw new NotImplementedException();
        }

        public async Task<Room> GetRoomByID(int id)
        {
            try
            {
                var HostelManagementContext = new HostelManagementContext();
                return await HostelManagementContext.Rooms
                    .Include(r => r.Hostel)
                        .ThenInclude(r => r.HostelOwnerEmailNavigation)
                    .Include(r => r.RoomPics)
                    .Include(r => r.Rents)
                    .FirstOrDefaultAsync(m => m.RoomId == id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<Room>> GetRoomsList()
        {
            try
            {
                var HostelManagementContext = new HostelManagementContext();
                return await HostelManagementContext.Rooms
                    .Include(r => r.Hostel)
                        .ThenInclude(h => h.HostelOwnerEmailNavigation)
                    .Include(r => r.RoomPics)
                    .Include(r => r.Rents)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<Room>> GetRoomsOfAHostel(int hostelId)
        {
            try
            {
                var HostelManagementContext = new HostelManagementContext();
                return await HostelManagementContext.Rooms
                        .Include(r => r.Hostel)
                        .Include(r => r.RoomPics)
                        .Include(r => r.Rents)
                        .Where(r => r.HostelId == hostelId)
                        .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateRoom(Room room)
        {
            try
            {
                var HostelManagementContext = new HostelManagementContext();
                HostelManagementContext.Attach(room).State = EntityState.Modified;
                await HostelManagementContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task ActivateRoom(int id)
        {
            try
            {
                var HostelManagementContext = new HostelManagementContext();
                var room = HostelManagementContext.Rooms.SingleOrDefault(h => h.RoomId.Equals(id));
                HostelManagementContext.Rooms.Attach(room);
                room.Status = 1;
                await HostelManagementContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DenyRoom(int id)
        {
            try
            {
                var HostelManagementContext = new HostelManagementContext();
                var room = HostelManagementContext.Rooms.SingleOrDefault(h => h.RoomId.Equals(id));
                HostelManagementContext.Rooms.Attach(room);
                room.Status = 3;
                await HostelManagementContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task PendingRoom(int id)
        {
            try
            {
                var HostelManagementContext = new HostelManagementContext();
                var room = HostelManagementContext.Rooms.SingleOrDefault(h => h.RoomId.Equals(id));
                HostelManagementContext.Rooms.Attach(room);
                room.Status = 0;
                await HostelManagementContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
