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
    public class WardRepository : IWardRepository
    {
        public async Task<IEnumerable<Ward>> GetWardListByDistrictId(int DistrictId)
            => await WardDAO.Instance.GetWardListByDistrictId(DistrictId);
    }
}
