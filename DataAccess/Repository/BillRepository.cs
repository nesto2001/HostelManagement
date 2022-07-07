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
        public async Task<Bill> GetBillByRentId(int RentId) => await BillDAO.Instance.GetBillByRentId(RentId);
    }
}
