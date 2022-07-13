using DataAccess.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HostelManagement.Pages
{
    [Authorize (Roles = "Owner")]
    public class HostelOwnerDashboard : PageModel
    {
        private readonly IHostelRepository hostelRepository;
        private readonly IRoomRepository roomRepository;
        private readonly IRentRepository rentRepository;
        private readonly IBillDetailRepository billDetailRepository;

        public HostelOwnerDashboard(IHostelRepository _hostelRepository,
            IRoomRepository _roomRepository,IRentRepository _rentRepository, IBillDetailRepository _billDetailRepository)
        {
            hostelRepository = _hostelRepository;
            roomRepository = _roomRepository;
            rentRepository = _rentRepository;
            billDetailRepository = _billDetailRepository;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            string Message = HttpContext.Session.GetString("HostelOwnerDashboardMessage");
            if (!String.IsNullOrEmpty(Message))
            {
                ViewData["HostelOwnerDashboardMessage"] = Message;
                HttpContext.Session.Remove("HostelOwnerDashboardMessage");
            }
            //hostel count, room count, rented room count
            var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            int UId = Int32.Parse(userId);
            var hostels = await hostelRepository.GetHostelsOfAnOwner(UId);
            ViewData["hostelCount"] = hostels.Count();
            var rooms = await roomRepository.GetRoomList();
            rooms = rooms.Where(r => r.Hostel.HostelOwnerEmailNavigation.UserId == UId);
            ViewData["roomCount"] = rooms.Count();
            ViewData["rentedRoomCount"] = rooms.Where(r => r.Rents.Count != 0).Count();
            //renting count, running rent, completed rent, pending start
            var rents = await rentRepository.GetRentList();
            rents = rents.Where(r => r.Room.Hostel.HostelOwnerEmailNavigation.UserId == UId);
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
            billDetails = billDetails.Where(b => b.Bill.Rent.Room.Hostel.HostelOwnerEmailNavigation.UserId == UId);
            var billDetailsYear = billDetails.Where(b => b.Bill.CreatedDate.Value.Year == DateTime.Now.Year);
            var billDetailsMonth = billDetailsYear.Where(b => b.Bill.CreatedDate.Value.Month == DateTime.Now.Month);
            ViewData["revenuesYear"] = billDetailsYear.Sum(b => b.Fee);
            ViewData["revenuesMonth"] = billDetailsMonth.Sum(b => b.Fee);
            return Page();
        }
    }
}
