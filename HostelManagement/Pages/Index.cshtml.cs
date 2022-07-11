using BusinessObject.BusinessObject;
using DataAccess.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace HostelManagement.Pages
{
    [AllowAnonymous]
    public class IndexModel : PageModel
    {
        private IProvinceRepository provinceRepository;
        private IDistrictRepository districtRepository;
        private IHostelRepository hostelRepository;
        public IndexModel(IProvinceRepository _provinceRepository, IDistrictRepository _districtRepository, IHostelRepository _hostelRepository)
        {
            provinceRepository = _provinceRepository;
            districtRepository = _districtRepository;
            hostelRepository = _hostelRepository;
        }
        public IEnumerable<Hostel> Hostels { get; set; }
        public async Task OnGetAsync(string searchKey, int sl_city, int sl_dist, int capacity)
        {
            ViewData["ProvinceId"] = new SelectList(await provinceRepository.GetProvincesList(), "ProvinceId", "ProvinceName");
            Hostels = await hostelRepository.GetHostelsList();

            IEnumerable<Hostel> HostelsSearchKey = Hostels;
            IEnumerable<Hostel> HostelsDistrictFilter = Hostels;
            IEnumerable<Hostel> HostelsCapaictyFilter = Hostels;
            if (!String.IsNullOrEmpty(searchKey))
            {
                HostelsSearchKey = HostelsSearchKey.Where(h => h.HostelName.Contains(searchKey)).ToList();
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
                        if (room.RomMaxCapacity >= capacity)
                        {
                            check++;
                        }
                    }
                    if (check == 0) HostelsCapaictyFilter = HostelsCapaictyFilter.Where(h => h != item).ToList();
                }
            }
            Hostels = HostelsSearchKey.Where(h => HostelsDistrictFilter.Contains(h)).Where(k => HostelsCapaictyFilter.Contains(k)).ToList();
        }

        public async Task<JsonResult> OnGetLoadDistrict(int ProvinceId)
        {
            IEnumerable<District> districts = await districtRepository.GetDistrictListByProvinceId(ProvinceId);
            return new JsonResult(districts);
        }
    }
}
