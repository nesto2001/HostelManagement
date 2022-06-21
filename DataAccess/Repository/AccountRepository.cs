using BusinessObject.BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private HostelManagementContext HostelManagementContext { get; set; }
        public AccountRepository(HostelManagementContext context)
        {
            HostelManagementContext = context;
        }
        public async Task AddAccount(Account Account)
        {
            try
            {
                HostelManagementContext.Attach(Account).State = EntityState.Added;
                await HostelManagementContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteAccount(Account Account)
        {
            try
            {
                HostelManagementContext.Accounts.Remove(Account);
                await HostelManagementContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Account> GetAccountByEmail(string email)
        {
            try
            {
                return await HostelManagementContext.Accounts.SingleOrDefaultAsync(account => account.UserEmail == email);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Account> GetAccountByID(int id)
        {
            try
            {
                return await HostelManagementContext.Accounts.SingleOrDefaultAsync(account => account.UserId == id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<Account>> GetAccountList()
        {
            try
            {
                return await HostelManagementContext.Accounts.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Account> GetLoginAccount(string email, string password)
        {
            try
            {
                return await HostelManagementContext.Accounts.SingleOrDefaultAsync(account => account.UserEmail == email && account.UserPassword == password);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateAccount(Account Account)
        {
            try
            {
                HostelManagementContext.Attach(Account).State = EntityState.Modified;
                await HostelManagementContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
