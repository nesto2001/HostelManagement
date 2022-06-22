using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObject.BusinessObject;
using DataAccess;

namespace HostelManagement.Pages.Hostels
{
    public class EditModel : PageModel
    {
        private readonly DataAccess.HostelManagementContext _context;

        public EditModel(DataAccess.HostelManagementContext context)
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
           ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
           ViewData["HostelOwnerEmail"] = new SelectList(_context.Accounts, "UserEmail", "PhoneNumber");
           ViewData["LocationId"] = new SelectList(_context.Locations, "LocationId", "AddressString");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Hostel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HostelExists(Hostel.HostelId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool HostelExists(int id)
        {
            return _context.Hostels.Any(e => e.HostelId == id);
        }
    }
}
