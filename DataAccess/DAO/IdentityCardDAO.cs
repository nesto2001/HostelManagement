using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.BusinessObject;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DAO
{
    public class IdentityCardDAO
    {
        private static IdentityCardDAO instance = null;
        private static readonly object instanceLock = new object();
        public static IdentityCardDAO Instance
        {
            get
            {
                lock(instanceLock)
                {
                    if (instance == null)
                        instance = new IdentityCardDAO();
                    return instance;
                }
            }
        }
        private IdentityCardDAO() { }
        public async Task AddIdCard(IdentityCard idCard)
        {
            try
            {
                var HostelManagementContext = new HostelManagementContext();
                HostelManagementContext.Attach(idCard).State = EntityState.Added;
                await HostelManagementContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteIdCard(IdentityCard idCard)
        {
            try
            {
                var HostelManagementContext = new HostelManagementContext();
                HostelManagementContext.IdentityCards.Remove(idCard);
                await HostelManagementContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task UpdateIdCard(IdentityCard idCard)
        {
            try
            {
                var HostelManagementContext = new HostelManagementContext();
                HostelManagementContext.Attach(idCard).State = EntityState.Modified;
                await HostelManagementContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IdentityCard> GetIdentityCardByID(string id)
        {
            try
            {
                var HostelManagementContext = new HostelManagementContext();
                return await HostelManagementContext.IdentityCards
                    .Include(h => h.Accounts)
                    .FirstOrDefaultAsync(idC => idC.IdCardNumber == id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
