using BusinessObject.BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IWardRepository
    {
        Task<IEnumerable<Ward>> GetWardListByDistrictId(int DistrictId);
    }
}
