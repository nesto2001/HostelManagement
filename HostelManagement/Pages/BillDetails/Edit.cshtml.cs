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

namespace HostelManagement.Pages.BillDetails
{
    public class EditModel : PageModel
    {
        private readonly DataAccess.HostelManagementContext _context;

        public EditModel(DataAccess.HostelManagementContext context)
        {
            _context = context;
        }

        [BindProperty]
        public BillDetail BillDetail { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            BillDetail = await _context.BillDetails
                .Include(b => b.Bill).FirstOrDefaultAsync(m => m.BillDetailId == id);

            if (BillDetail == null)
            {
                return NotFound();
            }
           ViewData["BillId"] = new SelectList(_context.Bills, "BillId", "BillId");
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

            _context.Attach(BillDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BillDetailExists(BillDetail.BillDetailId))
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

        private bool BillDetailExists(int id)
        {
            return _context.BillDetails.Any(e => e.BillDetailId == id);
        }
    }
}
