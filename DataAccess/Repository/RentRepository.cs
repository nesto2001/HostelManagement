﻿using BusinessObject.BusinessObject;
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
        public async Task<IEnumerable<Rent>> GetRentListByRoom(int roomId) => await RentDAO.Instance.GetRentListByRoom(roomId);
    }
}
