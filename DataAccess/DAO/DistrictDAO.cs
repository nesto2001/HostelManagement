using BusinessObject.BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class DistrictDAO
    {
        private static DistrictDAO instance = null;
        private static readonly object instanceLock = new object();
        public static DistrictDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new DistrictDAO();
                    }
                    return instance;
                }
            }
        }

        public async Task<IEnumerable<District>> GetDistrictListByProvinceId(int ProvinceId)
        {
            try
            {
                var HostelManagementContext = new HostelManagementContext();
                return await HostelManagementContext.Districts.Where(d => d.ProvinceId == ProvinceId).OrderBy(d => d.DistrictName).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
