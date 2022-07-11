using BusinessObject.BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class LocationDAO
    {
        private static LocationDAO instance = null;
        private static readonly object instanceLock = new object();
        public static LocationDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new LocationDAO();
                    }
                    return instance;
                }
            }
        }
        public async Task AddLocation(Location Location)
        {
            try
            {
                var HostelManagementContext = new HostelManagementContext();
                HostelManagementContext.Attach(Location).State = EntityState.Added;
                await HostelManagementContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Location> GetLocationByID(int id)
        {
            try
            {
                var HostelManagementContext = new HostelManagementContext();
                return await HostelManagementContext.Locations
                    .Include(l => l.Ward)
                        .ThenInclude(l => l.District)
                            .ThenInclude(l => l.Province)
                    .SingleOrDefaultAsync(location => location.LocationId == id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<Location>> GetLocationsList()
        {
            try
            {
                var HostelManagementContext = new HostelManagementContext();
                return await HostelManagementContext.Locations.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateLocation(Location location)
        {
            try
            {
                var HostelManagementContext = new HostelManagementContext();
                HostelManagementContext.Attach(location).State = EntityState.Modified;
                await HostelManagementContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
