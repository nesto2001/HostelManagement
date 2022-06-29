using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject.BusinessObject;
using DataAccess;

namespace HostelManagement.Pages.HostelPics
{
    public class DetailsModel : PageModel
    {
        private readonly DataAccess.HostelManagementContext _context;

        public DetailsModel(DataAccess.HostelManagementContext context)
        {
            _context = context;
        }

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
            return Page();
        }
    }
}
