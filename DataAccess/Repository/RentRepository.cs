using BusinessObject.BusinessObject;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class RentRepository : IRentRepository
    {
        public async Task AddRent(Rent Rent) => await RentDAO.Instance.AddRent(Rent);
        public async Task UpdateRent(Rent Rent) => await RentDAO.Instance.UpdateRent(Rent);
        public async Task<IEnumerable<Rent>> GetRentListByRoom(int roomId) => await RentDAO.Instance.GetRentListByRoom(roomId);
        public async Task<IEnumerable<Rent>> GetRentList() => await RentDAO.Instance.GetRentList();
        public async Task<Rent> GetRentByID(int id) => await RentDAO.Instance.GetRentByID(id);
    }
}
