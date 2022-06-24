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

        public Task<Location> GetLocationByID(int id)
        {
            throw new NotImplementedException();
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

        public Task UpdateLocation(Location Account)
        {
            throw new NotImplementedException();
        }
    }
}
