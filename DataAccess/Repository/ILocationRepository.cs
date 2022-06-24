using BusinessObject.BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface ILocationRepository
    {
        Task<Location> GetLocationByID(int id);
        Task UpdateLocation(Location Location);
        Task AddLocation(Location Location);
        Task<IEnumerable<Location>> GetLocationsList();
    }
}
