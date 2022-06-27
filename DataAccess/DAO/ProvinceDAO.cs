using BusinessObject.BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class ProvinceDAO
    {
        private static ProvinceDAO instance = null;
        private static readonly object instanceLock = new object();
        public static ProvinceDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new ProvinceDAO();
                    }
                    return instance;
                }
            }
        }

        public async Task<IEnumerable<Province>> GetProvincesList()
        {
            try
            {
                var HostelManagementContext = new HostelManagementContext();
                return await HostelManagementContext.Provinces.OrderBy(p => p.ProvinceName).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
