using BusinessObject.BusinessObject;
using DataAccess.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace HostelManagement.Pages.Hostels
{
    [Authorize(Roles = "Owner,Admin")]
    public class IndexModel : PageModel
    {
        private IHostelRepository hostelRepository;
        private IAccountRepository accountRepository;
        private IRentRepository rentRepository;

        public IndexModel(IHostelRepository _hostelRepository, IAccountRepository _accountRepository,
                            IRentRepository _rentRepository)
        {
            hostelRepository = _hostelRepository;
            accountRepository = _accountRepository;
            rentRepository = _rentRepository;
        }

        public IEnumerable<Hostel> Hostels { get; set; }

        public async Task OnGetAsync(string searchHostel)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            int UId = Int32.Parse(userId);
            var account = await accountRepository.GetAccountByID(UId);
            var role = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
            if (role == "Owner") Hostels = await hostelRepository.GetHostelsOfAnOwner(UId);
            else if (role == "Admin") Hostels = await hostelRepository.GetHostelsList();
            if (!String.IsNullOrEmpty(searchHostel))
            {
                Hostels = Hostels.Where(h => h.HostelName.ToLower().Contains(searchHostel.ToLower()));
            }
            ViewData["searchHostel"] = searchHostel;
        }

        public async Task<IActionResult> OnPostDeactivate(int id)
        {
            var Hostel =await hostelRepository.GetHostelByID(id);
            foreach (var item in Hostel.Rooms)
            {
                var rents = await rentRepository.GetRentListByRoom(item.RoomId);
                if (rents != null)
                {

                    foreach (var it in rents)
                    {
                        if (it.Status == 2 || it.Status == 5)
                        {
                            HttpContext.Session.SetString("AccessDeniedMessage", "Don't accept Inactive an hostel that exist room is renting.");
                            return RedirectToPage("../AccessDenied");
                        }
                    }
                }
            }
            await hostelRepository.DeactivateHostel(id);
            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnPostReactivate(int id)
        {
            await hostelRepository.ActivateHostel(id);
            return RedirectToPage("./Index");
        }
    }

}
