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
    public class DistrictRepository : IDistrictRepository
    {
        public async Task<IEnumerable<District>> GetDistrictListByProvinceId(int ProvinceId) 
            => await DistrictDAO.Instance.GetDistrictListByProvinceId(ProvinceId);
    }
}
