using BusinessObject.BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class LocationRepository : ILocationRepository
    {
        private HostelManagementContext HostelManagementContext { get; set; }
        public LocationRepository(HostelManagementContext context)
        {
            HostelManagementContext = context;
        }
        public async Task AddLocation(Location Location)
        {
            try
            {
                HostelManagementContext.Attach(Location).State = EntityState.Added;
                await HostelManagementContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Task DeleteLocation(Location Location)
        {
            throw new NotImplementedException();
        }

        public Task<Location> GetLocationByID(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Location>> GetLocationsList()
        {
            try
            {
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
