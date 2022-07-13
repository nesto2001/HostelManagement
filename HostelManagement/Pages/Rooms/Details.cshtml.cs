using BusinessObject.BusinessObject;
using DataAccess.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HostelManagement.Pages.Rooms
{
    [Authorize(Roles = "Owner,Admin")]
    public class DetailsModel : PageModel
    {
        private IRoomRepository roomRepository;
        private IHostelRepository hostelRepository;

        public DetailsModel(IRoomRepository _roomRepository, IHostelRepository _hostelRepository)
        {
            roomRepository = _roomRepository;
            hostelRepository = _hostelRepository;
        }

        public Room Room { get; set; }
        public String UserRole { get; set; }
        public Hostel hostel { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (HttpContext.User.Claims != null)
            {
                UserRole = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
            }


            Room = await roomRepository.GetRoomByID((int)id);
            hostel = await hostelRepository.GetHostelByID(Room.HostelId);
            HttpContext.Session.SetInt32("RoomView", (int)id);

            if (Room == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnGetApproveRoom(int id)
        {
            await roomRepository.ActivateRoom(id);
            return RedirectToPage("./Details",new { id = id });
        }

        public async Task<IActionResult> OnGetDenyRoom(int id)
        {
            await roomRepository.DenyRoom(id);
            return RedirectToPage("./Details", new { id = id });
        }
    }
}
