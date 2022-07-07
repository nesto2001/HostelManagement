using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject.BusinessObject;
using DataAccess;

namespace HostelManagement.Pages.BillDetails
{
    public class IndexModel : PageModel
    {
        private readonly DataAccess.HostelManagementContext _context;

        public IndexModel(DataAccess.HostelManagementContext context)
        {
            _context = context;
        }

        public IList<BillDetail> BillDetail { get;set; }

        public async Task OnGetAsync()
        {
            BillDetail = await _context.BillDetails
                .Include(b => b.Bill).ToListAsync();
        }
    }
}
