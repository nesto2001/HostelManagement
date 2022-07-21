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

namespace HostelManagement.Pages.Rents
{
    [Authorize(Roles = "Renter")]
    public class DeleteModel : PageModel
    {
        private IRentRepository rentRepository { get; }
        private IRoomRepository roomRepository { get; }
        public DeleteModel(IRentRepository _rentRepository, IRoomRepository _roomRepository)
        {
            rentRepository = _rentRepository;
            roomRepository = _roomRepository;
        }

        [BindProperty]
        public Rent Rent { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Rent = await rentRepository.GetRentByID((int)id);
            if (Rent.StartRentDate < DateTime.Now || Rent.Status != 0)
            {
                return RedirectToPage("../AccessDenied");
            }
            if (Rent == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Rent = await rentRepository.GetRentByID((int)id);

            if (Rent != null)
            {
                Rent.Status = 4;
                await rentRepository.UpdateRent(Rent);
                Room room = await roomRepository.GetRoomByID(Rent.RoomId);
                room.Status = 1;
                room.RoomCurrentCapacity = 0;
                await roomRepository.UpdateRoom(room);
            }

            return RedirectToPage("./Index");
        }
    }
}
