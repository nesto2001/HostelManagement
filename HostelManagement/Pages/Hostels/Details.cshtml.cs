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
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

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
        public string UserRole { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (HttpContext.User.Claims != null)
            {
                var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                //int UId = Int32.Parse(userId);
                UserRole = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
            }
            HttpContext.Session.SetInt32("HostelID", (int)id);
            Hostel = await hostelRepository.GetHostelByID((int)id);

            if (Hostel == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnGetYesAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (HttpContext.User.Claims != null)
            {
                var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                //int UId = Int32.Parse(userId);
                UserRole = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
            }
            HttpContext.Session.SetInt32("HostelID", (int)id);
            Hostel = await hostelRepository.GetHostelByID((int)id);
            Hostel.Status = 1;
            await hostelRepository.UpdateHostel(Hostel);
            if (Hostel == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnGetNoAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (HttpContext.User.Claims != null)
            {
                var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                //int UId = Int32.Parse(userId);
                UserRole = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
            }
            HttpContext.Session.SetInt32("HostelID", (int)id);
            Hostel = await hostelRepository.GetHostelByID((int)id);
            Hostel.Status = 3;
            await hostelRepository.UpdateHostel(Hostel);
            if (Hostel == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
