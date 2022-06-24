﻿using BusinessObject.BusinessObject;
using DataAccess.DAO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class ProvinceRepository : IProvinceRepository
    {
        public async Task<IEnumerable<Province>> GetProvincesList() => await ProvinceDAO.Instance.GetProvincesList();
    }
}
