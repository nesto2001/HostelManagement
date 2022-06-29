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

namespace HostelManagement.Pages.HostelPics
{
    public class EditModel : PageModel
    {
        private readonly DataAccess.HostelManagementContext _context;

        public EditModel(DataAccess.HostelManagementContext context)
        {
            _context = context;
        }

        [BindProperty]
        public HostelPic HostelPic { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            HostelPic = await _context.HostelPics
                .Include(h => h.Hostel).FirstOrDefaultAsync(m => m.HostelPicsId == id);

            if (HostelPic == null)
            {
                return NotFound();
            }
           ViewData["HostelId"] = new SelectList(_context.Hostels, "HostelId", "HostelOwnerEmail");
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

            _context.Attach(HostelPic).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HostelPicExists(HostelPic.HostelPicsId))
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

        private bool HostelPicExists(int id)
        {
            return _context.HostelPics.Any(e => e.HostelPicsId == id);
        }
    }
}
