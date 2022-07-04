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

namespace HostelManagement.Pages.Rents
{
    public class IndexModel : PageModel
    {
        private IAccountRepository accountRepository { get; }
        private IRentRepository rentRepository { get; }
        private IRoomRepository roomRepository { get; }
        private IRoomMemberRepository roomMemberRepository { get; }
        public IndexModel(IAccountRepository _accountRepository, IRentRepository _rentRepository,
                            IRoomRepository _roomRepository, IRoomMemberRepository _roomMemberRepository)
        {
            accountRepository = _accountRepository;
            rentRepository = _rentRepository;
            roomRepository = _roomRepository;
            roomMemberRepository = _roomMemberRepository;
        }

        public IEnumerable<Rent> Rents { get;set; }

        public async Task<ActionResult> OnGetAsync()
        {
            Rents = await rentRepository.GetRentList();
            int UId = 0;
            var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId != null)
            {
                UId = Int32.Parse(userId);
            } else
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
                Rents = await rentRepository.GetRentList();
            } else if (userRole.Equals("Owner"))
            {
                Rents = Rents.Where(r => r.Room.Hostel.HostelOwnerEmailNavigation.UserId == UId);
            } else if (userRole.Equals("Renter"))
            {
                Rents = Rents.Where(r => r.RentedByNavigation.UserId == UId);
            } else
            {
                return RedirectToPage("/AccessDenied");
            }
            return Page();
        }
    }
}
