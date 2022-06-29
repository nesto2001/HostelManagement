using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject.BusinessObject;
using DataAccess;

namespace HostelManagement.Pages.Locations
{
    public class DeleteModel : PageModel
    {
        private readonly DataAccess.HostelManagementContext _context;

        public DeleteModel(DataAccess.HostelManagementContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Location Location { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Location = await _context.Locations
                .Include(l => l.Ward).FirstOrDefaultAsync(m => m.LocationId == id);

            if (Location == null)
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

            Location = await _context.Locations.FindAsync(id);

            if (Location != null)
            {
                _context.Locations.Remove(Location);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
