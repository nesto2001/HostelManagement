using BusinessObject.BusinessObject;
using DataAccess.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HostelManagement.Pages.Hostels
{
    [AllowAnonymous]
    public class DetailsModel : PageModel
    {
        private IHostelRepository hostelRepository;
        private ICategoryRepository categoryRepository;
        private IProvinceRepository provinceRepository;
        private ILocationRepository locationRepository;
        private IHostelPicRepository hostelPicRepository;
        private IRoomRepository roomRepository;

        public DetailsModel(IHostelRepository _hostelRepository,
            ICategoryRepository _categoryRepository, IProvinceRepository _provinceRepository,
            ILocationRepository _locationRepository, IHostelPicRepository _hostelPicRepository, IRoomRepository _roomRepository)
        {
            hostelRepository = _hostelRepository;
            categoryRepository = _categoryRepository;
            provinceRepository = _provinceRepository;
            locationRepository = _locationRepository;
            hostelPicRepository = _hostelPicRepository;
            roomRepository = _roomRepository;
        }


        [BindProperty]
        public Hostel Hostel { get; set; }
        [BindProperty]
        public Location Location { get; set; }
        public IEnumerable<Room> Rooms { get; set; }

        public HostelPic HostelPic { get; set; }

        public IEnumerable<HostelPic> HostelPics { get; set; }

        public Status RoomStatus { get; set; }
        public class Status
        {
            public int StatusInt { get; set; }
            public string StatusType { get; set; }
        }



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
            HttpContext.Session.SetInt32("HostelID", (int)id);
            if (Hostel == null)
            {
                return NotFound();
            }
            Location = await locationRepository.GetLocationByID(Hostel.Location.LocationId);
            Rooms = await roomRepository.GetRoomsOfAHostel((int)id);
            HostelPics = await hostelPicRepository.GetHostelPicsOfAHostel((int)id);
            IEnumerable<Status> StatusList = new List<Status>
            {
                new Status{StatusInt=1, StatusType="Active"},
                new Status{StatusInt=2, StatusType="Inactive"},
                new Status{StatusInt=4, StatusType="Occupied"}
            };
            ViewData["ProvinceId"] = new SelectList(await provinceRepository.GetProvincesList(), "ProvinceId", "ProvinceName");
            ViewData["CategoryId"] = new SelectList(categoryRepository.GetCategoriesList().Result.Where(a => a.CategoryId == Hostel.CategoryId), "CategoryId", "CategoryName");
            ViewData["StatusId"] = new SelectList(StatusList, "StatusInt", "StatusType");
            ViewData["LocationId"] = Hostel.LocationId;
            ViewData["HostelOwnerEmail"] = Hostel.HostelOwnerEmail;
            return Page();
        }

        public async Task<IActionResult> OnGetYesAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (HttpContext.User.Claims != null)
            {
                var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                //int UId = Int32.Parse(userId);
            }
            HttpContext.Session.SetInt32("HostelID", (int)id);
            Hostel = await hostelRepository.GetHostelByID((int)id);
            Hostel.Status = 1;
            await hostelRepository.UpdateHostel(Hostel);
            if (Hostel == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnGetNoAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (HttpContext.User.Claims != null)
            {
                var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                //int UId = Int32.Parse(userId);
            }
            HttpContext.Session.SetInt32("HostelID", (int)id);
            Hostel = await hostelRepository.GetHostelByID((int)id);
            Hostel.Status = 3;
            await hostelRepository.UpdateHostel(Hostel);
            if (Hostel == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnGetApproveHostel(int id)
        {
            await hostelRepository.ActivateHostel(id);
            return RedirectToPage("./Details", new {id = id});
        }

        public async Task<IActionResult> OnGetDenyHostel(int id)
        {
            await hostelRepository.DenyHostel(id);
            return RedirectToPage("./Details", new { id = id });
        }
    }
}
