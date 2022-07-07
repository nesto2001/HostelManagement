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
    public class DetailsModel : PageModel
    {
        private readonly DataAccess.HostelManagementContext _context;

        public DetailsModel(DataAccess.HostelManagementContext context)
        {
            _context = context;
        }

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
            return Page();
        }
    }
}
