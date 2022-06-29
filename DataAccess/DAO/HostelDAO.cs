using BusinessObject.BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class HostelDAO
    {
        private static HostelDAO instance = null;
        private static readonly object instanceLock = new object();
        public static HostelDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new HostelDAO();
                    }
                    return instance;
                }
            }
        }
        public async Task AddHostel(Hostel hostel)
        {
            try
            {
                var HostelManagementContext = new HostelManagementContext();
                HostelManagementContext.Attach(hostel).State = EntityState.Added;
                await HostelManagementContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteHostel(Hostel hostel)
        {
            throw new NotImplementedException();
        }

        public async Task<Hostel> GetHostelByID(int id)
        {
            try
            {
                var HostelManagementContext = new HostelManagementContext();
                return await HostelManagementContext.Hostels
                    .Include(h => h.Rooms)
                    .Include(h => h.Category)
                    .Include(h => h.HostelOwnerEmailNavigation)
                    .Include(h => h.Location)
                        .ThenInclude(h => h.Ward)
                            .ThenInclude(h => h.District)
                                .ThenInclude(h => h.Province)
                    .Include(h => h.HostelPics)
                    .FirstOrDefaultAsync(hostel => hostel.HostelId == id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<Hostel>> GetHostelsList()
        {
            try
            {
                var HostelManagementContext = new HostelManagementContext();
                return await HostelManagementContext.Hostels
                    .Include(h => h.Rooms)
                    .Include(h => h.Category)
                    .Include(h => h.Location)
                        .ThenInclude(h => h.Ward)
                            .ThenInclude(h => h.District)
                    .Include(h => h.HostelPics)
                    .Include(h => h.HostelOwnerEmailNavigation)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateHostel(Hostel hostel)
        {
            try
            {
                var HostelManagementContext = new HostelManagementContext();
                HostelManagementContext.Attach(hostel).State = EntityState.Modified;
                await HostelManagementContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<Hostel>> GetHostelsOfAnOwner(int id)
        {
            try
            {
                var HostelManagementContext = new HostelManagementContext();
                return await HostelManagementContext.Hostels
                    .Include(h => h.Rooms)
                    .Include(h => h.Category)
                    .Include(h => h.Location)
                        .ThenInclude(h => h.Ward)
                            .ThenInclude(h => h.District)
                    .Include(h => h.HostelPics)
                    .Include(h => h.HostelOwnerEmailNavigation)
                        //.ThenInclude(h => h.UserId)
                    .Where(h => h.HostelOwnerEmailNavigation.UserId == id)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
