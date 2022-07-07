using BusinessObject.BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class BillDetailDAO
    {
        private static BillDetailDAO instance = null;
        private static readonly object instanceLock = new object();
        public static BillDetailDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new BillDetailDAO();
                    }
                    return instance;
                }
            }
        }

        public async Task AddBillDetail(BillDetail billDetail)
        {
            try
            {
                var HostelManagementContext = new HostelManagementContext();
                HostelManagementContext.Attach(billDetail).State = EntityState.Added;
                await HostelManagementContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<BillDetail>> GetBillDetailList()
        {
            try
            {
                var HostelManagementContext = new HostelManagementContext();
                return await HostelManagementContext.BillDetails
                    .Include(b => b.Bill)
                        .ThenInclude(b => b.Rent)
                            .ThenInclude(b => b.Room)
                                .ThenInclude(b => b.Hostel)
                                    .ThenInclude(b => b.HostelOwnerEmailNavigation)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
