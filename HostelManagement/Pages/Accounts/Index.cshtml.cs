using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject.BusinessObject;
using DataAccess.Data;

namespace HostelManagement.Pages.Accounts
{
    public class IndexModel : PageModel
    {
        private readonly DataAccess.Data.HostelManagementDBContext _context;

        public IndexModel(DataAccess.Data.HostelManagementDBContext context)
        {
            _context = context;
        }

        public IList<Account> Account { get;set; }

        public async Task OnGetAsync()
        {
            Account = await _context.Accounts
                .Include(a => a.IdCardNumberNavigation).ToListAsync();
        }
    }
}
