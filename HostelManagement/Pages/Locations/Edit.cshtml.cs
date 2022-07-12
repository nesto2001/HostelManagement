using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObject.BusinessObject;
using DataAccess;
using DataAccess.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace HostelManagement.Pages.Locations
{
    [Authorize(Roles ="Owner")]
    public class EditModel : PageModel
    {
        private ILocationRepository locationRepository; 
        private IProvinceRepository provinceRepository;
        private IDistrictRepository districtRepository;
        private IWardRepository wardRepository;

        public EditModel(ILocationRepository _locationRepository, IProvinceRepository _provinceRepository,
            IDistrictRepository _districtRepository, IWardRepository _wardRepository)
        {
            locationRepository = _locationRepository;
            provinceRepository = _provinceRepository;
            districtRepository = _districtRepository;
            wardRepository = _wardRepository;
        }


        [BindProperty]
        public Location Location { get; set; }
        public int HostelID { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            HostelID = (int)HttpContext.Session.GetInt32("HostelID");
            Location = await locationRepository.GetLocationByID((int)id);
            ViewData["ProvinceId"] = new SelectList(await provinceRepository.GetProvincesList(), "ProvinceId", "ProvinceName");
            if (Location == null)
            {
                return NotFound();
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            await locationRepository.UpdateLocation(Location);

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
