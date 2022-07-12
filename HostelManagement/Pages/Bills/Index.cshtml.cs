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
using Microsoft.AspNetCore.Authorization;

namespace HostelManagement.Pages.Bills
{
    [AllowAnonymous]
    public class IndexModel : PageModel
    {
        private IAccountRepository accountRepository { get; }
        private IRentRepository rentRepository { get; }
        private IRoomRepository roomRepository { get; }
        private IRoomMemberRepository roomMemberRepository { get; }
        private IBillRepository billRepository { get; }
        private IBillDetailRepository billDetailRepository { get; }
        public IndexModel(IAccountRepository _accountRepository, IRentRepository _rentRepository,
                            IRoomRepository _roomRepository, IRoomMemberRepository _roomMemberRepository,
                            IBillRepository _billRepository, IBillDetailRepository _billDetailRepository)
        {
            accountRepository = _accountRepository;
            rentRepository = _rentRepository;
            roomRepository = _roomRepository;
            roomMemberRepository = _roomMemberRepository;
            billRepository = _billRepository;
            billDetailRepository = _billDetailRepository;
        }


        public IEnumerable<Bill> Bill { get;set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Bill = await billRepository.GetBillList();
            Bill = Bill.Where(bill => bill.RentId == id);
            int UId = 0;
            var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId != null)
            {
                UId = Int32.Parse(userId);
            }
            else
            {
                return RedirectToPage("/AccessDenied");
            }
            var userRole = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
            if (userRole == null)
            {
                return RedirectToPage("/AccessDenied");
            }
            else
            {
                Bill.OrderByDescending(r => r.CreatedDate);
            }
            return Page();
        }
    }
}
