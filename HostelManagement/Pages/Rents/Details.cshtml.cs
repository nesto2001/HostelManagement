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

namespace HostelManagement.Pages.Rents
{
    public class DetailsModel : PageModel
    {
        private IAccountRepository accountRepository { get; }
        private IRentRepository rentRepository { get; }
        private IRoomRepository roomRepository { get; }
        private IRoomMemberRepository roomMemberRepository { get; }
        public DetailsModel(IAccountRepository _accountRepository, IRentRepository _rentRepository,
                            IRoomRepository _roomRepository, IRoomMemberRepository _roomMemberRepository)
        {
            accountRepository = _accountRepository;
            rentRepository = _rentRepository;
            roomRepository = _roomRepository;
            roomMemberRepository = _roomMemberRepository;
        }

        public Rent Rent { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
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

            if (roomMember.IsPresentator == true) roomMember.IsPresentator = false;
            else roomMember.IsPresentator = true;
            await roomMemberRepository.UpdateRoomMember(roomMember);

            return RedirectToPage("./Details", new {id= roomMember.RentId});
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
            if (Rent.IsDeposited == 1) Rent.Status = 1;

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
    }
}
