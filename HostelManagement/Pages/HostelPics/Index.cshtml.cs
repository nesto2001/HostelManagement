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
    public class IndexModel : PageModel
    {
        private readonly DataAccess.HostelManagementContext _context;

        public IndexModel(DataAccess.HostelManagementContext context)
        {
            _context = context;
        }

        public IList<HostelPic> HostelPic { get;set; }

        public async Task OnGetAsync()
        {
            HostelPic = await _context.HostelPics
                .Include(h => h.Hostel).ToListAsync();
        }
    }
}
