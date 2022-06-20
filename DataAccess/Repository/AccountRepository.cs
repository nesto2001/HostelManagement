using BusinessObject.BusinessObject;
using DataAccess.Data;
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
        private HostelManagementDBContext hostelManagementDBContext { get; set; }
        public AccountRepository(HostelManagementDBContext context)
        {
            hostelManagementDBContext = context;
        }
        public async Task AddAccount(Account Account)
        {
            try
            {
                hostelManagementDBContext.Attach(Account).State = EntityState.Added;
                await hostelManagementDBContext.SaveChangesAsync();
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
                hostelManagementDBContext.Accounts.Remove(Account);
                await hostelManagementDBContext.SaveChangesAsync();
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
                return await hostelManagementDBContext.Accounts.SingleOrDefaultAsync(account => account.UserEmail == email);
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
                return await hostelManagementDBContext.Accounts.SingleOrDefaultAsync(account => account.UserId == id);
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
                return await hostelManagementDBContext.Accounts.ToListAsync();
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
                return await hostelManagementDBContext.Accounts.SingleOrDefaultAsync(account => account.UserEmail == email && account.UserPassword == password);
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
                hostelManagementDBContext.Attach(Account).State = EntityState.Modified;
                await hostelManagementDBContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
