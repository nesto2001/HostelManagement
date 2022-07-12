using BusinessObject.BusinessObject;
using DataAccess.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HostelManagement.Pages.Hostels
{
    [Authorize(Roles = "Admin")]
    public class RequestsModel : PageModel
    {
        private IHostelRepository hostelRepository;
        private IRoomRepository roomRepository;
        private IAccountRepository accountRepository;

        public RequestsModel(IHostelRepository _hostelRepository, IAccountRepository _accountRepository, IRoomRepository _roomRepository)
        {
            hostelRepository = _hostelRepository;
            accountRepository = _accountRepository;
            roomRepository = _roomRepository;
        }

        public IEnumerable<Hostel> Hostels { get; set; }
        public IEnumerable<Room> Rooms { get; set; }

        public async Task OnGetAsync(string searchHostel)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            int UId = Int32.Parse(userId);
            var account = await accountRepository.GetAccountByID(UId);
            var role = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
            if (role == "Admin")
            {
                Hostels = await hostelRepository.GetHostelsList();
                Rooms = await roomRepository.GetRoomList();
            }
            Hostels = Hostels.Where(h => h.Status == 0);
            Rooms = Rooms.Where(r => r.Status == 0);
            if (!String.IsNullOrEmpty(searchHostel))
            {
                Hostels = Hostels.Where(h => h.HostelName.ToLower().Contains(searchHostel.ToLower()));
            }
            ViewData["searchHostel"] = searchHostel;
        }

    }
}
