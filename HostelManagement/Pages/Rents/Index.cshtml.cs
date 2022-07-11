using BusinessObject.BusinessObject;
using DataAccess.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HostelManagement.Pages.Rents
{
    public class IndexModel : PageModel
    {
        private IAccountRepository accountRepository { get; }
        private IRentRepository rentRepository { get; }
        private IRoomRepository roomRepository { get; }
        private IHostelRepository hostelRepository { get; }
        private IRoomMemberRepository roomMemberRepository { get; }
        public IndexModel(IAccountRepository _accountRepository, IRentRepository _rentRepository,
                            IRoomRepository _roomRepository, IRoomMemberRepository _roomMemberRepository, IHostelRepository _hostelRepository)
        {
            accountRepository = _accountRepository;
            rentRepository = _rentRepository;
            roomRepository = _roomRepository;
            roomMemberRepository = _roomMemberRepository;
            hostelRepository = _hostelRepository;
        }

        public IEnumerable<Rent> Rents { get; set; }

        public IEnumerable<Hostel> Hostels { get; set; }

        public IEnumerable<DisplayRoom> Rooms { get; set; }
        public class DisplayRoom
        {
            public int RoomId { get; set; }
            public string RoomTitle { get; set; }
        }

        public async Task<ActionResult> OnGetAsync(int? id)
        {
            Rents = await rentRepository.GetRentList();
            if (id != null)
            {
                Rents = Rents.Where(r => r.RoomId == (int)id);
            }
            int UId = 0;
            var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId != null)
            {
                UId = Int32.Parse(userId);
            }
            else
            {
                return RedirectToPage("/AccessDenied");
            }
            var userRole = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
            if (userRole == null)
            {
                return RedirectToPage("/AccessDenied");
            }
            else if (userRole.Equals("Admin"))
            {
                Rents = Rents.OrderByDescending(r => r.StartRentDate);
            }
            else if (userRole.Equals("Owner"))
            {
                Hostels = await hostelRepository.GetHostelsOfAnOwner(UId);
                ViewData["HostelId"] = new SelectList(Hostels, "HostelId", "HostelName");
            }
            else if (userRole.Equals("Renter"))
            {
                Rents = Rents.Where(r => r.RentedByNavigation.UserId == UId);
                Rents = Rents.OrderByDescending(r => r.StartRentDate);
            }
            else
            {
                return RedirectToPage("/AccessDenied");
            }
            return Page();
        }

        public async Task<ActionResult> OnPostAsync(int slHostel, int slRoom)
        {
            Rents = await rentRepository.GetRentListByRoom(slRoom); 
            int UId = 0;
            var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId != null)
            {
                UId = Int32.Parse(userId);
            }
            Hostels = await hostelRepository.GetHostelsOfAnOwner(UId);
            ViewData["HostelId"] = new SelectList(Hostels, "HostelId", "HostelName");
            ViewData["HostelName"] = hostelRepository.GetHostelByID(slHostel).Result.HostelName;
            ViewData["RoomName"] = roomRepository.GetRoomByID(slRoom).Result.RoomTitle;
            return Page();
        }

        public async Task<JsonResult> OnGetLoadRoom(int HostelId)
        {
            IEnumerable<Room> rooms = await roomRepository.GetRoomsOfAHostel(HostelId);
            var roomlist = rooms.Select(s => new
            {
                RoomId = s.RoomId,
                RoomName = s.RoomTitle
            });

            foreach (var item in rooms)
            {
            }
            return new JsonResult(roomlist);
        }
    }
}
