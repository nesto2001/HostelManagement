using BusinessObject.BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IAccountRepository
    {
        Task<Account> GetLoginAccount(string email, string password);
        Task<Account> GetAccountByID(int id);
        Task<Account> GetAccountByEmail(string email);
        Task UpdateAccount(Account Account);
        Task DeleteAccount(Account Account);
        Task AddAccount(Account Account);
        Task<IEnumerable<Account>> GetAccountList();
        Task ActivateUser(int id);
        Task InactivateUser(int id);
    }
}
