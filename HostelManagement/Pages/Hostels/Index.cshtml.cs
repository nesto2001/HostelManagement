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
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace HostelManagement.Pages.Hostels
{
    [Authorize]

    public class IndexModel : PageModel
    {
        private IHostelRepository hostelRepository;
        private IAccountRepository accountRepository;

        public IndexModel(IHostelRepository _hostelRepository, IAccountRepository _accountRepository)
        {
            hostelRepository = _hostelRepository;
            accountRepository = _accountRepository;
        }

        public IEnumerable<Hostel> Hostels { get;set; }

        public async Task OnGetAsync()
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            int UId = Int32.Parse(userId);
            var account = await accountRepository.GetAccountByID(UId);
            var role = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
            if (role == "Owner") Hostels = await hostelRepository.GetHostelsOfAnOwner(UId);
            else if (role == "Admin") Hostels = await hostelRepository.GetHostelsList();
        }
    }
}
