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
    public class DeleteModel : PageModel
    {
        private readonly DataAccess.HostelManagementContext _context;

        public DeleteModel(DataAccess.HostelManagementContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Hostel = await _context.Hostels.FindAsync(id);

            if (Hostel != null)
            {
                _context.Hostels.Remove(Hostel);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
