using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObject.BusinessObject;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using DataAccess.Repository;
using Microsoft.AspNetCore.Http;

namespace HostelManagement.Pages.Hostels
{
    public class CreateModel : PageModel
    {
        private IHostelRepository hostelRepository;
        private IAccountRepository accountRepository;
        private ICategoryRepository categoryRepository;
        private IProvinceRepository provinceRepository;
        private IDistrictRepository districtRepository;
        private IWardRepository wardRepository;
        private ILocationRepository locationRepository;

        public CreateModel(IHostelRepository _hostelRepository, IAccountRepository _accountRepository,
            ICategoryRepository _categoryRepository, IProvinceRepository _provinceRepository,
            IDistrictRepository _districtRepository, IWardRepository _wardRepository,
            ILocationRepository _locationRepository)
        {
            hostelRepository = _hostelRepository;
            accountRepository = _accountRepository;
            categoryRepository = _categoryRepository;
            provinceRepository = _provinceRepository;
            districtRepository = _districtRepository;
            wardRepository = _wardRepository;
            locationRepository = _locationRepository;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var locas = await locationRepository.GetLocationsList();
            locID = locas.Count() + 1;
            HttpContext.Session.SetInt32("locID", locID);
            ViewData["CategoryId"] = new SelectList(await categoryRepository.GetCategoriesList(), "CategoryId", "CategoryName");
            ViewData["HostelOwnerEmail"] = new SelectList(await accountRepository.GetAccountList(), "UserEmail", "PhoneNumber");
            //ViewData["LocationId"] = new SelectList(_context.Locations, "LocationId", "AddressString");
            ViewData["ProvinceId"] = new SelectList(await provinceRepository.GetProvincesList(), "ProvinceId", "ProvinceName");
            return Page();
        }

        [BindProperty]
        public Hostel Hostel { get; set; }
        [BindProperty]
        public Location Location { get; set; }
        public int locID { get; set; }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            locID = (int)HttpContext.Session.GetInt32("locID");
            Hostel.LocationId = locID;
            Location.LocationId = locID;
            await locationRepository.AddLocation(Location);
            await hostelRepository.AddHostel(Hostel);

            return RedirectToPage("./Index");
        }

        public async Task<JsonResult> OnGetLoadDistrict(int ProvinceId)
        {
            IEnumerable<District> districts = await districtRepository.GetDistrictListByProvinceId(ProvinceId);
            return new JsonResult(districts);
        }

        public async Task<JsonResult> OnGetLoadWard(int DistrictId)
        {
            IEnumerable<Ward> wards = await wardRepository.GetWardListByDistrictId(DistrictId);
            return new JsonResult(wards);
        }

    }
}
