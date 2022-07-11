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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace HostelManagement.Pages.Rooms
{
    [Authorize(Roles = "Owner,Admin")]
    public class DetailsModel : PageModel
    {
        private IRoomRepository roomRepository;

        public DetailsModel(IRoomRepository _roomRepository)
        {
            roomRepository = _roomRepository;
        }

        public Room Room { get; set; }
        public String UserRole { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (HttpContext.User.Claims != null) {            
                UserRole = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
            }


            Room = await roomRepository.GetRoomByID((int)id);
            HttpContext.Session.SetInt32("RoomView", (int)id);
            
            if (Room == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
