using BusinessObject.BusinessObject;
using DataAccess.Repository;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace HostelManagement.Pages
{
    [AllowAnonymous]
    public class SearchModel : PageModel
    {
        private IProvinceRepository provinceRepository;
        private IDistrictRepository districtRepository;
        private IHostelRepository hostelRepository;
        public SearchModel(IProvinceRepository _provinceRepository, IDistrictRepository _districtRepository, IHostelRepository _hostelRepository)
        {
            provinceRepository = _provinceRepository;
            districtRepository = _districtRepository;
            hostelRepository = _hostelRepository;
        }
        public IEnumerable<Hostel> Hostels { get; set; }
        public int capacityChoosen { get; set; }
        public async Task OnGetAsync(string searchKey, int sl_city, int sl_dist, int capacity)
        {
            string userId, role;
            int UId = 0;
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                UId = Int32.Parse(userId);
                role = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
            }
            ViewData["ProvinceId"] = new SelectList(await provinceRepository.GetProvincesList(), "ProvinceId", "ProvinceName");
            Hostels = await hostelRepository.GetHostelsList();
            Hostels = Hostels.Where(hos => hos.Status == 1);
            capacityChoosen = capacity;
            IEnumerable<Hostel> HostelsSearchKey = Hostels;
            IEnumerable<Hostel> HostelsDistrictFilter = Hostels;
            IEnumerable<Hostel> HostelsCapaictyFilter = Hostels;
            if (!String.IsNullOrEmpty(searchKey))
            {
                HostelsSearchKey = HostelsSearchKey.Where(h => h.HostelName.ToLower().Contains(searchKey.ToLower())).ToList();
            }
            if (sl_city != 0)
            {
                HostelsDistrictFilter = HostelsDistrictFilter.Where(h => h.Location.Ward.District.Province.ProvinceId == sl_city).ToList();
            }
            if (sl_dist != 0)
            {
                HostelsDistrictFilter = HostelsDistrictFilter.Where(h => h.Location.Ward.District.DistrictId == sl_dist).ToList();
            }
            if (capacity != 0)
            {
                foreach (var item in Hostels)
                {
                    int check = 0;
                    foreach (var room in item.Rooms)
                    {
                        if (room.RomMaxCapacity >= capacity && room.Status == 1)
                        {
                            check++;
                        }
                    }
                    if (check == 0) HostelsCapaictyFilter = HostelsCapaictyFilter.Where(h => h != item).ToList();
                }
            }
            Hostels = HostelsSearchKey.Where(h => HostelsDistrictFilter.Contains(h)).Where(k => HostelsCapaictyFilter.Contains(k)).ToList();
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                Hostels = Hostels.Where(h => h.HostelOwnerEmailNavigation.UserId != UId);
            }
        }
    }
}
