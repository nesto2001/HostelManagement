using BusinessObject.BusinessObject;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class BillRepository : IBillRepository
    {
        public async Task AddBill(Bill bill) => await BillDAO.Instance.AddBill(bill); 
        public async Task UpdateBill(Bill bill) => await BillDAO.Instance.UpdateBill(bill); 
        public async Task<Bill> GetBillById(int BillId) => await BillDAO.Instance.GetBillById(BillId);
        public async Task<IEnumerable<Bill>> GetBillList() => await BillDAO.Instance.GetBillList();
    }
}
