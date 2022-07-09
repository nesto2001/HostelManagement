using BusinessObject.BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IBillRepository
    {
        Task AddBill(Bill bill); 
        Task UpdateBill(Bill bill); 
        Task<Bill> GetBillById(int BillId);
        Task<IEnumerable<Bill>> GetBillList();
    }
}
