using BusinessObject.BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class WardRepository : IWardRepository
    {
        private HostelManagementContext HostelManagementContext { get; set; }
        public WardRepository(HostelManagementContext context)
        {
            HostelManagementContext = context;
        }
        public async Task<IEnumerable<Ward>> GetWardListByDistrictId(int DistrictId)
        {
            try
            {
                return await HostelManagementContext.Wards.Where(d => d.DistrictId == DistrictId).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
