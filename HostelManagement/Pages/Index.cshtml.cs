using BusinessObject.BusinessObject;
using DataAccess.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HostelManagement.Pages
{
    public class IndexModel : PageModel
    {
        private IProvinceRepository provinceRepository;
        private IDistrictRepository districtRepository;
        public IndexModel(IProvinceRepository _provinceRepository, IDistrictRepository _districtRepository)
        {
            provinceRepository = _provinceRepository;
            districtRepository = _districtRepository;
        }

        public async Task OnGetAsync(string searchKey, int sl_city, int sl_dist, int capacity)
        {
            ViewData["ProvinceId"] = new SelectList(await provinceRepository.GetProvincesList(), "ProvinceId", "ProvinceName");
            if (!String.IsNullOrEmpty(searchKey))
            {
                if (sl_city != 0)
                {
                    if (sl_dist != 0)
                    {
                        if (capacity != 0)
                        {

                        }
                    }
                }
            }
        }
        public async Task<JsonResult> OnGetLoadDistrict(int ProvinceId)
        {
            IEnumerable<District> districts = await districtRepository.GetDistrictListByProvinceId(ProvinceId);
            return new JsonResult(districts);
        }
    }
}
