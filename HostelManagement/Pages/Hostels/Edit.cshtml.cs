using BusinessObject.BusinessObject;
using DataAccess.Repository;
using HostelManagement.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace HostelManagement.Pages.Hostels
{
    [Authorize(Roles = "Owner")]
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
            ViewData["CategoryId"] = new SelectList(await categoryRepository.GetCategoriesList(), "CategoryId", "CategoryName");
            ViewData["StatusId"] = new SelectList(StatusList, "StatusInt", "StatusType");
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
            Hostel.Status = 0;
            await hostelRepository.UpdateHostel(Hostel);
            foreach (var item in Hostel.Rooms)
            {
                await roomRepository.PendingRoom(item.RoomId);
            }
            return RedirectToPage("./Details", new { id = Hostel.HostelId });
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

        public async Task<IActionResult> OnPostUploadhostel()
        {
            int countPic = 0;
            if (FileUploads != null)
            {
                HostelPic.HostelId = Hostel.HostelId;
                HostelPic.Hostel = Hostel;
                int i = 1;
                countPic = FileUploads.Count();
                foreach (var FileUpload in FileUploads)
                {
                    HostelPic.HostelPicUrl = await Utilities.UploadFile(FileUpload, @"images\hostels\", FileUpload.FileName);
                }
            }
            return Page();
        }

        public async Task<IActionResult> OnPostUpdateLocation()
        {
            ModelState.ClearValidationState("Description");
            ModelState.ClearValidationState("HostelName");
            foreach (var item in ModelState)
            {
                if (item.Value.ValidationState.Equals("Invalid"))
                {
                    return Page();
                }
            }
            int HosID = (int)HttpContext.Session.GetInt32("HostelID");
            Hostel = await hostelRepository.GetHostelByID(HosID);
            Hostel.Status = 0;
            await locationRepository.UpdateLocation(Location);
            return RedirectToPage("./Edit", new { id = Hostel.HostelId });
        }

        public async Task<IActionResult> OnPostUpdateHostel()
        {
            ModelState.ClearValidationState("AddressString");
            foreach (var item in ModelState)
            {
                if (item.Value.ValidationState.Equals("Invalid"))
                {
                    return Page();
                }
            }
            Hostel.Status = 0;
            await hostelRepository.UpdateHostel(Hostel);
            return RedirectToPage("./Edit", new { id = Hostel.HostelId });
        }

        public async Task<IActionResult> OnGetRemoveimage(int id, int hostelId)
        {
            var pic = await hostelPicRepository.GetHostelPic(id);
            await hostelPicRepository.DeleteHostelPic(pic);
            return RedirectToPage("./Edit", new { id = hostelId });
        }


    }
}
