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
    public class CategoryRepository : ICategoryRepository
    {
        public async Task<IEnumerable<Category>> GetCategoriesList() => await CategoryDAO.Instance.GetCategoriesList();
    }
}
