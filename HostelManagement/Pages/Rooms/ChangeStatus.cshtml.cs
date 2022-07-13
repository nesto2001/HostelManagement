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
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace HostelManagement.Pages.Rooms
{
    [Authorize(Roles = "Owner,Admin")]
    public class ChangeStatusModel : PageModel
    {
        private IHostelRepository hostelRepository;
        private IRoomRepository roomRepository;

        public ChangeStatusModel(IHostelRepository _hostelRepository, IRoomRepository _roomRepository)
        {
            hostelRepository = _hostelRepository;
            roomRepository = _roomRepository;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostPendingAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Room = await roomRepository.GetRoomByID((int)id);

            if (Room != null)
            {
                Room.Status = 0;
                await roomRepository.UpdateRoom(Room);
                
            }
            return RedirectToPage("./Details", new { id = Room.RoomId });
        }
        public async Task<IActionResult> OnPostActiveAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Room = await roomRepository.GetRoomByID((int)id);

            if (Room != null)
            {
                Room.Status = 1;
                await roomRepository.UpdateRoom(Room);

            }
            return RedirectToPage("./Details", new { id = Room.RoomId });
        }

        public async Task<IActionResult> OnPostInactiveAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Room = await roomRepository.GetRoomByID((int)id);
            foreach (var it in Room.Rents){
                if (it.Status == 1 || it.Status == 2 || it.Status == 5)
                {
                    HttpContext.Session.SetString("AccessDeniedMessage", "Don't accept Inactive an room that exist room is renting.");
                    return RedirectToPage("../AccessDenied");
                }
            }
            if (Room != null)
            {
                Room.Status = 2;
                await roomRepository.UpdateRoom(Room);

            }
            return RedirectToPage("./Details", new { id = Room.RoomId });
        }

        public async Task<IActionResult> OnPostDeniedAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Room = await roomRepository.GetRoomByID((int)id);

            if (Room != null)
            {
                Room.Status = 3;
                await roomRepository.UpdateRoom(Room);

            }
            return RedirectToPage("./Details", new { id = Room.RoomId });
        }
    }
}
