using DataAccess.Repository;
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
    public class HostelOwnerDashboard : PageModel
    {
        private readonly IHostelRepository hostelRepository;
        private readonly IAccountRepository accountRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IProvinceRepository provinceRepository;
        private readonly IDistrictRepository districtRepository;
        private readonly IWardRepository wardRepository;
        private readonly ILocationRepository locationRepository;
        private readonly IHostelPicRepository hostelPicRepository;
        private readonly IRoomRepository roomRepository;
        private readonly IRoomMemberRepository roomMemberRepository;
        private readonly IRentRepository rentRepository;

        public HostelOwnerDashboard(IHostelRepository _hostelRepository, IAccountRepository _accountRepository,
            ICategoryRepository _categoryRepository, IProvinceRepository _provinceRepository,
            IDistrictRepository _districtRepository, IWardRepository _wardRepository,
            ILocationRepository _locationRepository, IHostelPicRepository _hostelPicRepository, 
            IRoomRepository _roomRepository, IRoomMemberRepository _roomMemberRepository, IRentRepository _rentRepository)
        {
            hostelRepository = _hostelRepository;
            accountRepository = _accountRepository;
            categoryRepository = _categoryRepository;
            provinceRepository = _provinceRepository;
            districtRepository = _districtRepository;
            wardRepository = _wardRepository;
            locationRepository = _locationRepository;
            hostelPicRepository = _hostelPicRepository;
            roomRepository = _roomRepository;
            roomMemberRepository = _roomMemberRepository;
            rentRepository = _rentRepository;
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
            ViewData["runningRent"] = rents.Count(r => r.Status == 1 || r.Status == 2);
            ViewData["completedRent"] = rents.Count(r => r.Status == 3 || r.Status == 4);
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

            return Page();
        }
    }
}
