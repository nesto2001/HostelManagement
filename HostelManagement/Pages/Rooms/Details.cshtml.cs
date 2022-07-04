﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject.BusinessObject;
using DataAccess;
using DataAccess.Repository;

namespace HostelManagement.Pages.Rooms
{
    public class DetailsModel : PageModel
    {
        private IRoomRepository roomRepository;

        public DetailsModel(IRoomRepository _roomRepository)
        {
            roomRepository = _roomRepository;
        }

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
            return Page();
        }
    }
}
