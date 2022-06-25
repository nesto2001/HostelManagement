using BusinessObject.BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                throw new NotImplementedException();
            }

            public async Task<IEnumerable<Room>> GetRoomsList()
            {
                try
                {
                    var HostelManagementContext = new HostelManagementContext();
                    return await HostelManagementContext.Rooms.ToListAsync();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            public async Task UpdateRoom(Room Room)
            {
                throw new NotImplementedException();
            }
        }
}
