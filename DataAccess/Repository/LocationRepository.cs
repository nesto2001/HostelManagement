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
        public async Task<Location> GetLocationByID(int id) => await LocationDAO.Instance.GetLocationByID(id);

        public async Task<IEnumerable<Location>> GetLocationsList() => await LocationDAO.Instance.GetLocationsList();

        public async Task UpdateLocation(Location location) => await LocationDAO.Instance.UpdateLocation(location);
    }
}
