using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject.BusinessObject;
using DataAccess.Data;

namespace HostelManagement.Pages.Accounts
{
    public class DetailsModel : PageModel
    {
        private readonly DataAccess.Data.HostelManagementDBContext _context;

        public DetailsModel(DataAccess.Data.HostelManagementDBContext context)
        {
            _context = context;
        }

        public Account Account { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Account = await _context.Accounts
                .Include(a => a.IdCardNumberNavigation).FirstOrDefaultAsync(m => m.UserId == id);

            if (Account == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
