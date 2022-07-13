using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObject.BusinessObject;
using DataAccess;
using DataAccess.Repository;
using HostelManagement.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace HostelManagement.Pages.Bills
{
    [Authorize(Roles ="Owner")]
    public class CreateModel : PageModel
    {
        private IRentRepository rentRepository { get; }
        private IBillRepository billRepository { get; }
        private IBillDetailRepository billDetailRepository { get; }
        private ISendMailService sendMailService { get; }
        public CreateModel( IRentRepository _rentRepository, IBillRepository _billRepository, 
            IBillDetailRepository _billDetailRepository, ISendMailService _sendMailService)
        {
            rentRepository = _rentRepository;
            billRepository = _billRepository;
            billDetailRepository = _billDetailRepository;
            sendMailService = _sendMailService;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return RedirectToPage("../AccessDenied");
            }
            rent = await rentRepository.GetRentByID((int)id);
            var LastBill = rent.StartRentDate;
            if (rent.Bills.Count() != 0)
            {
                LastBill = (DateTime)rent.Bills.Max(b => b.CreatedDate);
            }
            if (LastBill > DateTime.Now.AddDays(-15))
            {
                HttpContext.Session.SetString("HostelOwnerDashboardMessage", "This contract have already exist bill in recent months.");
                return RedirectToPage("../Rents/Details", new {id= id });
            }
            rent.RentedByNavigation.Rents = null;
            rent.Room = null;
            rent.RoomMembers = null;
            rent.Bills = null;
            SessionHelper.SetObjectAsJson(HttpContext.Session, "rentGenerate", rent);
            if (rent.StartRentDate.Day > DateTime.Now.Day)
            {
                dateRentIssued = new DateTime(DateTime.Now.Year, DateTime.Now.AddMonths(-1).Month, rent.StartRentDate.Day);
            }
            dateRentIssued = new DateTime(DateTime.Now.Year, DateTime.Now.Month, rent.StartRentDate.Day);
            return Page();
        }

        [BindProperty]
        public Bill Bill { get; set; }

        [BindProperty]
        public BillDetail[] BillDetail { get; set; }
        public DateTime dateRentIssued { get; set; }
        public Rent rent { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                rent = SessionHelper.GetObjectFromJson<Rent>(HttpContext.Session, "rentGenerate");
                rent.RentedByNavigation.Rents = null;
                rent.Room = null;
                rent.RoomMembers = null;
                rent.Bills = null;
                if (rent.StartRentDate.Day > DateTime.Now.Day)
                {
                    dateRentIssued = new DateTime(DateTime.Now.Year, DateTime.Now.AddMonths(-1).Month, rent.StartRentDate.Day);
                }
                dateRentIssued = new DateTime(DateTime.Now.Year, DateTime.Now.Month, rent.StartRentDate.Day);
                return Page();
            }
            Rent rentGenerate = SessionHelper.GetObjectFromJson<Rent>(HttpContext.Session, "rentGenerate");
            Bill.RentId = rentGenerate.RentId;
            Bill.RoomId = rentGenerate.RoomId;
            Bill.StartRentDate = rentGenerate.StartRentDate;
            Bill.EndRentDate = rentGenerate.EndRentDate;
            Bill.CreatedDate = DateTime.Now;
            await billRepository.AddBill(Bill);
            if (rentGenerate.StartRentDate.Day > DateTime.Now.Day)
            {
                dateRentIssued = new DateTime(DateTime.Now.Year, DateTime.Now.AddMonths(-1).Month, rentGenerate.StartRentDate.Day);
            }
            dateRentIssued = new DateTime(DateTime.Now.Year, DateTime.Now.Month, rentGenerate.StartRentDate.Day);
            foreach (var item in BillDetail)
            {
                item.BillId = Bill.BillId;
            }
            BillDetail[0].BillDescription = "Room usage fee";
            BillDetail[0].Fee = rentGenerate.Total;
            BillDetail[0].DateIssued = dateRentIssued;
            await billDetailRepository.AddBillDetail(BillDetail[0]);
            BillDetail[1].BillDescription = "Monthly electricity usage fee";
            await billDetailRepository.AddBillDetail(BillDetail[1]);
            BillDetail[2].BillDescription = "Monthly water usage fee";
            await billDetailRepository.AddBillDetail(BillDetail[2]);
            BillDetail[3].BillDescription = "Other fee";
            await billDetailRepository.AddBillDetail(BillDetail[3]);
            string body = "Thank you for your service hostel renting. \n" +
                    "Your bill is created at " + DateTime.Now + "\n" +
                    "Please pay bill before: " + Bill.DueDate + "\n" +
                    "Thank you. \n" +
                    "Please send your feedback by reply mail.\n" +
                    "Best Regard,";
            await sendMailService.SendEmailAsync(rentGenerate.RentedBy, "Bill for contract of room", body);
            return RedirectToPage("../BillDetails/Index");
        }
    }
}
