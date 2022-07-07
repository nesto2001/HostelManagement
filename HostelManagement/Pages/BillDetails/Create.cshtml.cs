using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObject.BusinessObject;
using DataAccess;

namespace HostelManagement.Pages.BillDetails
{
    public class CreateModel : PageModel
    {
        private readonly DataAccess.HostelManagementContext _context;

        public CreateModel(DataAccess.HostelManagementContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["BillId"] = new SelectList(_context.Bills, "BillId", "BillId");
            return Page();
        }

        [BindProperty]
        public BillDetail BillDetail { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.BillDetails.Add(BillDetail);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
