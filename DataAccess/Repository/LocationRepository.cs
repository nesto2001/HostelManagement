using BusinessObject.BusinessObject;
using DataAccess.DAO;
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
        public async Task AddLocation(Location Location) => await LocationDAO.Instance.AddLocation(Location);
        public Task<Location> GetLocationByID(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Location>> GetLocationsList() => await LocationDAO.Instance.GetLocationsList();

        public Task UpdateLocation(Location Account)
        {
            throw new NotImplementedException();
        }
    }
}
