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
    public class DetailsModel : PageModel
    {
        private readonly DataAccess.HostelManagementContext _context;

        public DetailsModel(DataAccess.HostelManagementContext context)
        {
            _context = context;
        }

        public Hostel Hostel { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Hostel = await _context.Hostels
                .Include(h => h.Category)
                .Include(h => h.HostelOwnerEmailNavigation)
                .Include(h => h.Location).FirstOrDefaultAsync(m => m.HostelId == id);

            if (Hostel == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
