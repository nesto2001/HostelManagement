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

namespace HostelManagement.Pages.Bills
{
    public class DetailsModel : PageModel
    {
        private IAccountRepository accountRepository { get; }
        private IRentRepository rentRepository { get; }
        private IRoomRepository roomRepository { get; }
        private IRoomMemberRepository roomMemberRepository { get; }
        private IBillRepository billRepository { get; }
        private IBillDetailRepository billDetailRepository { get; }
        public DetailsModel(IAccountRepository _accountRepository, IRentRepository _rentRepository,
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


        public Bill Bill { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var bills = await billRepository.GetBillList();
            Bill = bills.First(m => m.BillId == id);
            if (Bill == null)
            {
                return NotFound();
            }
            ViewData["sum"] = Bill.BillDetails.Sum(bd => bd.Fee);
            return Page();
        }
    }
}
