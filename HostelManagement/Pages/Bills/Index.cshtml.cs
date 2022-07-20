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
using HostelManagement.Helpers;
using Microsoft.AspNetCore.Http;

namespace HostelManagement.Pages.Bills
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private IBillRepository billRepository { get; }
        private ISendMailService sendMailService { get; }
        public IndexModel(IBillRepository _billRepository, ISendMailService _sendMailService)
        {
            billRepository = _billRepository;
            sendMailService = _sendMailService;
        }


        public IEnumerable<Bill> Bill { get;set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!String.IsNullOrEmpty(HttpContext.Session.GetString("sendmail")))
            {
                ViewData["mailMessage"] = "Send mail successfully";
                HttpContext.Session.Remove("sendmail");
            }
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
            Bill = await billRepository.GetBillList();
            Bill = Bill
                .Where(bill => bill.Rent.Room.Hostel.HostelOwnerEmailNavigation.UserId == UId)
                .Where(bill => (bill.DueDate != bill.CreatedDate && bill.DueDate < DateTime.Now));
            
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

        public async Task<IActionResult> OnGetSendMailAsync(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            string body = "Dear, \n" +
                    "You delay your bill. Please pay bill soon. (" +
                    "). Thank you. \n" +
                    "Please send your feedback by reply mail.\n" +
                    "Best Regard,";
            await sendMailService.SendEmailAsync(id, "Remind pay bill", body);
            HttpContext.Session.SetString("sendmail", "Send mail successfully");
            return RedirectToPage("./Index");
        }
    }
}
