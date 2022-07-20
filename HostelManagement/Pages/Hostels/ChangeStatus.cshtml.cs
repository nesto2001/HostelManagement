using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject.BusinessObject;
using DataAccess;
using DataAccess.Repository;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HostelManagement.Pages.Hostels
{
    [Authorize(Roles = "Owner,Admin")]
    public class ChangeStatusModel : PageModel
    {
        private IHostelRepository hostelRepository;
        private ICategoryRepository categoryRepository;
        private IProvinceRepository provinceRepository;
        private ILocationRepository locationRepository;
        private IHostelPicRepository hostelPicRepository;
        private IRoomRepository roomRepository;
        private IRentRepository rentRepository;

        public ChangeStatusModel(IHostelRepository _hostelRepository,
            ICategoryRepository _categoryRepository, IProvinceRepository _provinceRepository,
            ILocationRepository _locationRepository, IHostelPicRepository _hostelPicRepository, IRoomRepository _roomRepository,
            IRentRepository _rentRepository)
        {
            hostelRepository = _hostelRepository;
            categoryRepository = _categoryRepository;
            provinceRepository = _provinceRepository;
            locationRepository = _locationRepository;
            hostelPicRepository = _hostelPicRepository;
            roomRepository = _roomRepository;
            rentRepository = _rentRepository;
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

        public async Task<IActionResult> OnPostPendingAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Hostel = await hostelRepository.GetHostelByID((int)id);

            if (Hostel != null)
            {
                Hostel.Status = 0;
                await hostelRepository.UpdateHostel(Hostel);
                
            }
            return RedirectToPage("./Details", new { id = Hostel.HostelId });
        }

        public async Task<IActionResult> OnPostActiveAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Hostel = await hostelRepository.GetHostelByID((int)id);

            if (Hostel != null)
            {
                Hostel.Status = 1;
                await hostelRepository.UpdateHostel(Hostel);

            }
            return RedirectToPage("./Details", new { id = Hostel.HostelId });
        }

        public async Task<IActionResult> OnPostInactiveAsync(int? id)
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
            foreach (var item in Hostel.Rooms)
            {
                var rents = await rentRepository.GetRentListByRoom(item.RoomId);
                if (rents != null)
                {

                    foreach (var it in rents)
                    {
                        if (it.Status == 1 || it.Status == 2 || it.Status == 5)
                        {
                            HttpContext.Session.SetString("AccessDeniedMessage", "Don't accept Inactive an hostel that exist room is renting.");
                            return RedirectToPage("../AccessDenied");
                        }
                    }
                }
            }
            if (Hostel != null)
            {
                Hostel.Status = 2;
                await hostelRepository.UpdateHostel(Hostel);

            }
            return RedirectToPage("./Details", new { id = Hostel.HostelId });
        }

        public async Task<IActionResult> OnPostDeniedAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Hostel = await hostelRepository.GetHostelByID((int)id);

            if (Hostel != null)
            {
                Hostel.Status = 3;
                await hostelRepository.UpdateHostel(Hostel);
                foreach (var item in Hostel.Rooms)
                {
                    await roomRepository.DenyRoom(item.RoomId);
                }
            }
            return RedirectToPage("./Details", new { id = Hostel.HostelId });
        }
    }
}
