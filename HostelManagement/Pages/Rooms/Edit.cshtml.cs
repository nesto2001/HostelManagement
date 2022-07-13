using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObject.BusinessObject;
using DataAccess;
using DataAccess.Repository;
using Microsoft.AspNetCore.Authorization;

namespace HostelManagement.Pages.Rooms
{
    [Authorize(Roles = "Owner")]
    public class EditModel : PageModel
    {
        private IRoomRepository roomRepository;
        private IHostelRepository hostelRepository;

        public EditModel(IRoomRepository _roomRepository, IHostelRepository _hostelRepository)
        {
            roomRepository = _roomRepository;
            hostelRepository = _hostelRepository;
        }

        [BindProperty]
        public Room Room { get; set; }
        public Hostel hostel { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Room = await roomRepository.GetRoomByID((int)id);
            if (Room == null)
            {
                return NotFound();
            }
            hostel = await hostelRepository.GetHostelByID(Room.HostelId);
            ViewData["HostelId"] = Room.HostelId;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ViewData["HostelId"] = Room.HostelId;
                return Page();
            }
            Room.Status = 0;
            await roomRepository.UpdateRoom(Room);
            return RedirectToPage("./Details", new { id = Room.RoomId });
        }
    }
}
