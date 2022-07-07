using BusinessObject.BusinessObject;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class BillDetailRepository : IBillDetailRepository
    {
        public async Task AddBillDetail(BillDetail billDetail) => await BillDetailDAO.Instance.AddBillDetail(billDetail);
        public async Task<IEnumerable<BillDetail>> GetBillDetailList() => await BillDetailDAO.Instance.GetBillDetailList();
    }
}
