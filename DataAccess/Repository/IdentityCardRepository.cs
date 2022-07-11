using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.BusinessObject;
using DataAccess.DAO;

namespace DataAccess.Repository
{
    public class IdentityCardRepository : IIdentityCardRepository
    {
        public async Task AddIdCard(IdentityCard idCard) => await IdentityCardDAO.Instance.AddIdCard(idCard);
        public async Task DeleteIdCard(IdentityCard idCard) => await IdentityCardDAO.Instance.DeleteIdCard(idCard);
        public async Task UpdateIdCard(IdentityCard idCard) => await IdentityCardDAO.Instance.UpdateIdCard(idCard);
        public async Task<IdentityCard> GetIdentityCardByID(string id) => await IdentityCardDAO.Instance.GetIdentityCardByID(id);
    }
}
