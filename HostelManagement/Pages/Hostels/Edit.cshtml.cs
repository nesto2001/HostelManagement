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
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace HostelManagement.Pages.Hostels
{
    public class EditModel : PageModel
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

        public EditModel(IHostelRepository _hostelRepository, IAccountRepository _accountRepository,
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
        public Hostel Hostel { get; set; }

        [Required(ErrorMessage = "Please chose at least one file.")]
        [DataType(DataType.Upload)]
        //[FileExtensions(Extensions = "png,jpg,jpeg,gif")]
        [Display(Name = "Choose Hostel images(s)")]
        [BindProperty]
        public IFormFile[] FileUploads { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Hostel = await hostelRepository.GetHostelByID((int)id);
            if (Hostel == null)
            {
                return NotFound();
            }

            ViewData["CategoryId"] = new SelectList(await categoryRepository.GetCategoriesList(), "CategoryId", "CategoryName");
            ViewData["LocationId"] = Hostel.LocationId;
            ViewData["HostelOwnerEmail"] = Hostel.HostelOwnerEmail;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ViewData["CategoryId"] = new SelectList(await categoryRepository.GetCategoriesList(), "CategoryId", "CategoryName");
                ViewData["LocationId"] = Hostel.LocationId;
                ViewData["HostelOwnerEmail"] = Hostel.HostelOwnerEmail;
                return Page();
            }

            await hostelRepository.UpdateHostel(Hostel);
            return RedirectToPage("./Details", new {id = Hostel.HostelId});
        }

    }
}
