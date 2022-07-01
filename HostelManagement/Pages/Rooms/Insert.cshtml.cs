using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessObject.BusinessObject;
using DataAccess.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HostelManagement.Pages.Rooms
{
    public class InsertModel : PageModel
    {
        private IHostelRepository hostelRepository;
        private IAccountRepository accountRepository;
        private ICategoryRepository categoryRepository;
        private IProvinceRepository provinceRepository;
        private IDistrictRepository districtRepository;
        private IWardRepository wardRepository;
        private ILocationRepository locationRepository;
        private IHostelPicRepository hostelPicRepository;
        private IRoomRepository roomRepository;

        public InsertModel(IHostelRepository _hostelRepository, IAccountRepository _accountRepository,
            ICategoryRepository _categoryRepository, IProvinceRepository _provinceRepository,
            IDistrictRepository _districtRepository, IWardRepository _wardRepository,
            ILocationRepository _locationRepository, IHostelPicRepository _hostelPicRepository, IRoomRepository _roomRepository)
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
        }


        [BindProperty]
        public Room Room { get; set; }
        //public int HostelId { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            HttpContext.Session.SetInt32("HostelId", id);
            return Page();
        }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            Room.HostelId = (int)HttpContext.Session.GetInt32("HostelId");
            await roomRepository.AddRoom(Room);
            return RedirectToPage("../Hostels/Details", new { id = Room.HostelId });
        }
    }
}
