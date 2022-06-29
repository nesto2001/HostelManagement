using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject.BusinessObject;
using DataAccess;
using DataAccess.Repository;

namespace HostelManagement.Pages.Hostels
{
    public class DetailsModel : PageModel
    {
        private IHostelRepository hostelRepository;

        public DetailsModel(IHostelRepository _hostelRepository)
        {
            hostelRepository = _hostelRepository;
        }

        public Hostel Hostel { get; set; }
        public IEnumerable<Room> Rooms { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Hostel = await hostelRepository.GetHostelByID((int)id);

            if (Hostel == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
