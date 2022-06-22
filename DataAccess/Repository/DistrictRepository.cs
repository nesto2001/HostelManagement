using BusinessObject.BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class DistrictRepository : IDistrictRepository
    {
        private HostelManagementContext HostelManagementContext { get; set; }
        public DistrictRepository(HostelManagementContext context)
        {
            HostelManagementContext = context;
        }
        public async Task<IEnumerable<District>> GetDistrictListByProvinceId(int ProvinceId)
        {
            try
            {
                return await HostelManagementContext.Districts.Where(d => d.ProvinceId == ProvinceId).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
