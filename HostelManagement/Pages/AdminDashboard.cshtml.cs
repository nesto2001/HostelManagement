using DataAccess.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HostelManagement.Pages
{
    [Authorize(Roles = "Admin")]
    public class AdminDashboardModel : PageModel
    {
        private readonly IHostelRepository hostelRepository;
        private readonly IRoomRepository roomRepository;
        private readonly IRentRepository rentRepository;
        private readonly IBillDetailRepository billDetailRepository;
        private readonly IAccountRepository accountRepository;

        public AdminDashboardModel(IHostelRepository _hostelRepository,
            IRoomRepository _roomRepository, IRentRepository _rentRepository, IBillDetailRepository _billDetailRepository,
            IAccountRepository _accountRepository)
        {
            hostelRepository = _hostelRepository;
            roomRepository = _roomRepository;
            rentRepository = _rentRepository;
            billDetailRepository = _billDetailRepository;
            accountRepository = _accountRepository;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            string Message = HttpContext.Session.GetString("AdminDashboardMessage");
            if (!String.IsNullOrEmpty(Message))
            {
                ViewData["HostelOwnerDashboardMessage"] = Message;
                HttpContext.Session.Remove("HostelOwnerDashboardMessage");
            }
            //hostel count, room count, rented room count
            var hostels = await hostelRepository.GetHostelsList();
            ViewData["hostelCount"] = hostels.Count();
            var rooms = await roomRepository.GetRoomList();
            ViewData["roomCount"] = rooms.Count();
            ViewData["rentedRoomCount"] = rooms.Where(r => r.Status == 4).Count();
            //renting count, running rent, completed rent, pending start
            var rents = await rentRepository.GetRentList();
            ViewData["rentingCount"] = rents.Count();
            ViewData["runningRent"] = rents.Count(r => r.Status == 1 || r.Status == 2 || r.Status == 5);
            ViewData["completedRent"] = rents.Count(r => r.Status == 3 || r.Status == 6);
            ViewData["pendingStartRent"] = rents.Count(r => r.Status == 0);
            //renter count, roomMember count
            List<string> renters = new List<string>();
            foreach (var item in rents)
            {
                if (renters == null) renters.Add(item.RentedBy);
                if (!renters.Contains(item.RentedBy)) renters.Add(item.RentedBy);
            }
            ViewData["renterCount"] = renters.Count();
            ViewData["roomMemberCount"] = rents.Sum(r => r.RoomMembers.Count());
            //revenues month, revenues year (bill)
            var billDetails = await billDetailRepository.GetBillDetailList();
            var billDetailsYear = billDetails.Where(b => b.Bill.CreatedDate.Value.Year == DateTime.Now.Year);
            var billDetailsMonth = billDetailsYear.Where(b => b.Bill.CreatedDate.Value.Month == DateTime.Now.Month);
            ViewData["revenuesYear"] = billDetailsYear.Sum(b => b.Fee);
            ViewData["revenuesMonth"] = billDetailsMonth.Sum(b => b.Fee);

            var accounts = await accountRepository.GetAccountList();
            ViewData["accountCount"] = accounts.Count();
            return Page();
        }
    }
}
