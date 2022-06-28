﻿using BusinessObject.BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IHostelRepository
    {
        Task<Hostel> GetHostelByID(int id);
        Task UpdateHostel(Hostel hostel);
        Task DeleteHostel(Hostel hostel);
        Task AddHostel(Hostel hostel);
        Task<IEnumerable<Hostel>> GetHostelsList();
        Task<IEnumerable<Hostel>> GetHostelsOfAnOwner(int id);
    }
}
