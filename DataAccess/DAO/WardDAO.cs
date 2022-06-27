using BusinessObject.BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class WardDAO
    {
        private static WardDAO instance = null;
        private static readonly object instanceLock = new object();
        public static WardDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new WardDAO();
                    }
                    return instance;
                }
            }
        }

        public async Task<IEnumerable<Ward>> GetWardListByDistrictId(int DistrictId)
        {
            try
            {
                var HostelManagementContext = new HostelManagementContext();
                return await HostelManagementContext.Wards.Where(d => d.DistrictId == DistrictId).OrderBy(w => w.WardName).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
