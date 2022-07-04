﻿using System;
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

namespace HostelManagement.Pages.Rooms
{
    public class EditModel : PageModel
    {
        private IRoomRepository roomRepository;

        public EditModel(IRoomRepository _roomRepository)
        {
            roomRepository = _roomRepository;
        }

        [BindProperty]
        public Room Room { get; set; }

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

            await roomRepository.UpdateRoom(Room);
            return RedirectToPage("./Details", new { id = Room.RoomId });
        }
    }
}
