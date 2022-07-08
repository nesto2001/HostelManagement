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

namespace HostelManagement.Pages.Bills
{
    public class CreateModel : PageModel
    {
        private IAccountRepository accountRepository { get; }
        private IRentRepository rentRepository { get; }
        private IRoomRepository roomRepository { get; }
        private IRoomMemberRepository roomMemberRepository { get; }
        private IBillRepository billRepository { get; }
        private IBillDetailRepository billDetailRepository { get; }
        public CreateModel(IAccountRepository _accountRepository, IRentRepository _rentRepository,
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

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return RedirectToPage("../AccessDenied");
            }
            rent = await rentRepository.GetRentByID((int)id);
            if (rent.StartRentDate < DateTime.Now.AddDays(15))
            {
                return RedirectToPage("../AccessDenied");
            }
            var LastBill = DateTime.Now.AddDays(-20);
            if (rent.Bills != null)
            {
                LastBill = (DateTime)rent.Bills.Max(b => b.CreatedDate);
            }
            if (LastBill > DateTime.Now.AddDays(-15))
            {
                HttpContext.Session.SetString("HostelOwnerDashboardMessage", "This contract have already exist bill in recent months.");
                return RedirectToPage("../HostelOwnerDashboard");
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
            return RedirectToPage("../BillDetails/Index");
        }
    }
}
