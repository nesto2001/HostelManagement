using BusinessObject.BusinessObject;
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
        private HostelManagementContext HostelManagementContext { get; set; }
        public HostelRepository(HostelManagementContext context)
        {
            HostelManagementContext = context;
        }
        public async Task AddHostel(Hostel hostel)
        {
            try
            {
                HostelManagementContext.Attach(hostel).State = EntityState.Added;
                await HostelManagementContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Task DeleteHostel(Hostel hostel)
        {
            throw new NotImplementedException();
        }

        public Task<Hostel> GetHostelByID(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Hostel>> GetHostelsList()
        {
            throw new NotImplementedException();
        }

        public Task UpdateHostel(Hostel Account)
        {
            throw new NotImplementedException();
        }
    }
}
