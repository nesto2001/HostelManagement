using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.BusinessObject;

namespace DataAccess.Repository
{
    public interface IIdentityCardRepository
    {
        Task AddIdCard(IdentityCard idCard);
        Task DeleteIdCard(IdentityCard idCard);
        Task UpdateIdCard(IdentityCard idCard);
        Task<IdentityCard> GetIdentityCardByID(string id);
    }
}
