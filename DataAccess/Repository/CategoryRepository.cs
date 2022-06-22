using BusinessObject.BusinessObject;
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
        private HostelManagementContext HostelManagementContext { get; set; }
        public CategoryRepository(HostelManagementContext context)
        {
            HostelManagementContext = context;
        }
        public async Task<IEnumerable<Category>> GetCategoriesList()
        {
            try
            {
                return await HostelManagementContext.Categories.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
