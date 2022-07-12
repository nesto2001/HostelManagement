using BusinessObject.BusinessObject;
using DataAccess.Repository;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace HostelManagement.Pages.Hostels
{
    [Authorize(Roles = "Admin")]
    public class RequestsModel : PageModel
    {
        private IHostelRepository hostelRepository;
        private IAccountRepository accountRepository;

        public RequestsModel(IHostelRepository _hostelRepository, IAccountRepository _accountRepository)
        {
            hostelRepository = _hostelRepository;
            accountRepository = _accountRepository;
        }

        public IEnumerable<Hostel> Hostels { get; set; }

        public async Task OnGetAsync(string searchHostel)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            int UId = Int32.Parse(userId);
            var account = await accountRepository.GetAccountByID(UId);
            var role = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
            if (role == "Admin") Hostels = await hostelRepository.GetHostelsList();
            Hostels = Hostels.Where(h => h.Status == 0);
            if (!String.IsNullOrEmpty(searchHostel))
            {
                Hostels = Hostels.Where(h => h.HostelName.ToLower().Contains(searchHostel.ToLower()));
            }
            ViewData["searchHostel"] = searchHostel;
        }

    }
}
