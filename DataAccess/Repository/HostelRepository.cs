using BusinessObject.BusinessObject;
using DataAccess.DAO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class HostelRepository : IHostelRepository
    {
        public async Task AddHostel(Hostel hostel) => await HostelDAO.Instance.AddHostel(hostel);
        public async Task DeleteHostel(Hostel hostel) => await HostelDAO.Instance.DeleteHostel(hostel);

        public async Task<Hostel> GetHostelByID(int id) => await HostelDAO.Instance.GetHostelByID(id);

        public async Task<IEnumerable<Hostel>> GetHostelsList() => await HostelDAO.Instance.GetHostelsList();
        public async Task<IEnumerable<Hostel>> GetHostelsOfAnOwner(int id) => await HostelDAO.Instance.GetHostelsOfAnOwner(id);

        public async Task UpdateHostel(Hostel hostel) => await HostelDAO.Instance.UpdateHostel(hostel);

        public async Task DeactivateHostel(int id) => await HostelDAO.Instance.DeactivateHostel(id);

        public async Task ActivateHostel(int id) => await HostelDAO.Instance.ActivateHostel(id);

        public async Task DenyHostel(int id) => await HostelDAO.Instance.DenyHostel(id);
    }
}
