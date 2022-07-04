using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject.BusinessObject;
using DataAccess;

namespace HostelManagement.Pages.Rents
{
    public class DetailsModel : PageModel
    {
        private readonly DataAccess.HostelManagementContext _context;

        public DetailsModel(DataAccess.HostelManagementContext context)
        {
            _context = context;
        }

        public Rent Rent { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Rent = await _context.Rents
                .Include(r => r.RentedByNavigation)
                .Include(r => r.Room).FirstOrDefaultAsync(m => m.RentId == id);

            if (Rent == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
