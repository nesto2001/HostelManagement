using BusinessObject.BusinessObject;
using DataAccess.DAO;
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
        public async Task AddAccount(Account Account) => await AccountDAO.Instance.AddAccount(Account);

        public async Task DeleteAccount(Account Account) => await AccountDAO.Instance.DeleteAccount(Account);

        public async Task<Account> GetAccountByEmail(string email) => await AccountDAO.Instance.GetAccountByEmail(email);

        public async Task<Account> GetAccountByID(int id) => await AccountDAO.Instance.GetAccountByID(id);

        public async Task<IEnumerable<Account>> GetAccountList() => await AccountDAO.Instance.GetAccountList();

        public async Task<Account> GetLoginAccount(string email, string password) => await AccountDAO.Instance.GetLoginAccount(email, password);

        public async Task UpdateAccount(Account Account) => await AccountDAO.Instance.UpdateAccount(Account);
        public async Task ActivateUser(int id) => await AccountDAO.Instance.ActivateUser(id);
        public async Task InactivateUser(int id) => await AccountDAO.Instance.InactiveUser(id);
    }
}
