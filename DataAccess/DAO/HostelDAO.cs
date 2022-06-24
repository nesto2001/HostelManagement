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
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Hostel>> GetHostelsList()
        {
            throw new NotImplementedException();
        }

        public async Task UpdateHostel(Hostel hostel)
        {
            throw new NotImplementedException();
        }
    }
}
