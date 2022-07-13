using BusinessObject.BusinessObject;
using DataAccess.Repository;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "Admin,Owner,Renter")]
    public class IndexModel : PageModel
    {
        private IRentRepository rentRepository { get; }
        private IRoomRepository roomRepository { get; }
        private IHostelRepository hostelRepository { get; }
        public IndexModel( IRentRepository _rentRepository,
                            IRoomRepository _roomRepository, IHostelRepository _hostelRepository)
        {
            rentRepository = _rentRepository;
            roomRepository = _roomRepository;
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
            Rents = Rents.OrderByDescending(r => r.IsDeposited).ThenByDescending(r => r.RentId);
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
                Rents = Rents.Where(r => r.Room.Hostel.HostelOwnerEmailNavigation.UserId == UId);
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
            if (slHostel != 0)
            {
                if (slRoom != 0)
                    Rents = await rentRepository.GetRentListByRoom(slRoom);
                else
                {
                    var rooms = await roomRepository.GetRoomsOfAHostel(slHostel);
                    Rents = await rentRepository.GetRentList();
                    Rents = Rents.Where(r => r.Room.HostelId == slHostel);
                    if (rooms == null)
                    {
                        Rents = await rentRepository.GetRentList();
                    }
                    //implement get all contract of each room and concat to create list contract of hostel
                }
            } else
            {
                Rents = await rentRepository.GetRentList();
            }
            
            int UId = 0;
            var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId != null)
            {
                UId = Int32.Parse(userId);
            }
            Hostels = await hostelRepository.GetHostelsOfAnOwner(UId);
            Rents = Rents.OrderByDescending(r => r.IsDeposited).ThenByDescending(r => r.RentId);
            ViewData["HostelId"] = new SelectList(Hostels, "HostelId", "HostelName");
            if (slHostel != 0) ViewData["HostelName"] = hostelRepository.GetHostelByID(slHostel).Result.HostelName;
            if (slRoom != 0) ViewData["RoomName"] = roomRepository.GetRoomByID(slRoom).Result.RoomTitle;
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
