using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject.BusinessObject;
using DataAccess;

namespace HostelManagement.Pages.Hostels
{
    public class IndexModel : PageModel
    {
        private readonly DataAccess.HostelManagementContext _context;

        public IndexModel(DataAccess.HostelManagementContext context)
        {
            _context = context;
        }

        public IList<Hostel> Hostel { get;set; }

        public async Task OnGetAsync()
        {
            Hostel = await _context.Hostels
                .Include(h => h.Category)
                .Include(h => h.HostelOwnerEmailNavigation)
                .Include(h => h.Location).ToListAsync();
        }
    }
}
