using BusinessObject.BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class ProvinceRepository : IProvinceRepository
    {
        private HostelManagementContext HostelManagementContext { get; set; }
        public ProvinceRepository(HostelManagementContext context)
        {
            HostelManagementContext = context;
        }
        public async Task<IEnumerable<Province>> GetProvincesList()
        {
            try
            {
                return await HostelManagementContext.Provinces.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
