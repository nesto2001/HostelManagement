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

namespace HostelManagement.Pages.Bills
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private IBillRepository billRepository { get; }
        public DetailsModel(IBillRepository _billRepository)
        {
            billRepository = _billRepository;
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

        public async Task<IActionResult> OnGetConfirmAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Bill = await billRepository.GetBillById((int)id);
            if (Bill == null)
            {
                return NotFound();
            }

            if (Bill.DueDate < Bill.CreatedDate) Bill.DueDate = Bill.CreatedDate;

            await billRepository.UpdateBill(Bill);

            return RedirectToPage("./Details", new { id = Bill.BillId });
        }

        public async Task<IActionResult> OnGetNotConfirmAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Bill = await billRepository.GetBillById((int)id);
            if (Bill == null)
            {
                return NotFound();
            }

            if (Bill.DueDate < Bill.CreatedDate) Bill.DueDate = DateTime.Parse(Bill.CreatedDate.ToString()).AddDays(1);

            await billRepository.UpdateBill(Bill);

            return RedirectToPage("./Details", new { id = Bill.BillId });
        }

        public async Task<IActionResult> OnGetCheckAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Bill = await billRepository.GetBillById((int)id);
            if (Bill == null)
            {
                return NotFound();
            }

            if (Bill.DueDate > Bill.CreatedDate) Bill.DueDate = DateTime.Parse(Bill.CreatedDate.ToString()).AddDays(-1);

            await billRepository.UpdateBill(Bill);

            return RedirectToPage("./Details", new { id = Bill.BillId });
        }
    }
}
