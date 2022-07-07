using BusinessObject.BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IBillDetailRepository
    {
        Task AddBillDetail(BillDetail billDetail); 
        Task<IEnumerable<BillDetail>> GetBillDetailList();
    }
}
