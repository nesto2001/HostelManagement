﻿using BusinessObject.BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IRentRepository
    {
        Task AddRent(Rent Rent);
        Task<IEnumerable<Rent>> GetRentListByRoom(int roomId);
    }
}
