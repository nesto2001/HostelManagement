using BusinessObject.BusinessObject;
using DataAccess.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;

namespace HostelManagement.Pages.Rents
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private IRentRepository rentRepository { get; }
        private IRoomRepository roomRepository { get; }
        private IHostelRepository hostelRepository { get; }
        private IRoomMemberRepository roomMemberRepository { get; }
        public DetailsModel( IRentRepository _rentRepository,
                            IRoomRepository _roomRepository, IRoomMemberRepository _roomMemberRepository, IHostelRepository _hostelRepository)
        {
            rentRepository = _rentRepository;
            roomRepository = _roomRepository;
            roomMemberRepository = _roomMemberRepository;
            hostelRepository = _hostelRepository;
        }

        public Rent Rent { get; set; }
        public Room Room { get; set; }
        public Hostel Hostel { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Rent = await rentRepository.GetRentByID((int)id);
            Room = await roomRepository.GetRoomByID(Rent.RoomId);
            Hostel = await hostelRepository.GetHostelByID(Room.HostelId);
            if (Rent == null)
            {
                return NotFound();
            }
            string Message = HttpContext.Session.GetString("HostelOwnerDashboardMessage");
            if (!String.IsNullOrEmpty(Message))
            {
                ViewData["HostelOwnerDashboardMessage"] = Message;
                HttpContext.Session.Remove("HostelOwnerDashboardMessage");
            }
            return Page();
        }

        public async Task<IActionResult> OnGetChangePresentAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomMember = await roomMemberRepository.GetRoomMemberByID((int)id);
            if (roomMember == null)
            {
                return NotFound();
            }
            var room = await roomRepository.GetRoomByID(roomMember.RoomId);
            if (roomMember.Status == 1)
            {
                roomMember.Status = 0;
                room.RoomCurrentCapacity = room.RoomCurrentCapacity - 1;
            }
            else
            {
                roomMember.Status = 1;
                room.RoomCurrentCapacity = room.RoomCurrentCapacity + 1;
            }
            await roomMemberRepository.UpdateRoomMember(roomMember);
            await roomRepository.UpdateRoom(room);
            return RedirectToPage("./Details", new { id = roomMember.RentId });
        }

        public async Task<IActionResult> OnGetDepositedCheckAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Rent = await rentRepository.GetRentByID((int)id);
            if (Rent == null)
            {
                return NotFound();
            }

            if (Rent.IsDeposited == 0) Rent.IsDeposited = 1;

            await rentRepository.UpdateRent(Rent);

            return RedirectToPage("./Details", new { id = Rent.RentId });
        }

        public async Task<IActionResult> OnGetDepositedConfirmAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Rent = await rentRepository.GetRentByID((int)id);
            if (Rent == null)
            {
                return NotFound();
            }

            if (Rent.IsDeposited == 1) Rent.IsDeposited = 2;

            await rentRepository.UpdateRent(Rent);

            return RedirectToPage("./Details", new { id = Rent.RentId });
        }

        public async Task<IActionResult> OnGetDepositedNoneAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Rent = await rentRepository.GetRentByID((int)id);
            if (Rent == null)
            {
                return NotFound();
            }

            if (Rent.IsDeposited == 1) Rent.IsDeposited = 0;

            await rentRepository.UpdateRent(Rent);

            return RedirectToPage("./Details", new { id = Rent.RentId });
        }

        public async Task<IActionResult> OnGetCloseAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Rent = await rentRepository.GetRentByID((int)id);
            if (Rent == null)
            {
                return NotFound();
            }
            if (Rent.Status == 1) Rent.Status = 6;

            await rentRepository.UpdateRent(Rent);

            return RedirectToPage("./Details", new { id = Rent.RentId });
        }
    }
}
