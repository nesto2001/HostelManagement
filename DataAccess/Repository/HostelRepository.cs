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
    public class HostelRepository : IHostelRepository
    {
        public async Task AddHostel(Hostel hostel) => await HostelDAO.Instance.AddHostel(hostel);
        public async Task DeleteHostel(Hostel hostel) => await HostelDAO.Instance.DeleteHostel(hostel);

        public async Task<Hostel> GetHostelByID(int id) => await HostelDAO.Instance.GetHostelByID(id);

        public async Task<IEnumerable<Hostel>> GetHostelsList() => await HostelDAO.Instance.GetHostelsList();
        public async Task<IEnumerable<Hostel>> GetHostelsOfAnOwner(int id) => await HostelDAO.Instance.GetHostelsOfAnOwner(id);

        public async Task UpdateHostel(Hostel hostel) => await HostelDAO.Instance.UpdateHostel(hostel);
    }
}
