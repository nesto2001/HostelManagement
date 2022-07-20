using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BusinessObject.BusinessObject;
using DataAccess.Repository;
using HostelManagement.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace HostelManagement.Pages.Rooms
{
    [Authorize (Roles = "Owner")]
    public class InsertModel : PageModel
    {
        private IRoomPicRepository roomPicRepository;
        private IRoomRepository roomRepository;
        private IHostelRepository hostelRepository;

        public InsertModel(IRoomPicRepository _roomPicRepository, IRoomRepository _roomRepository, IHostelRepository _hostelRepository)
        {
            roomPicRepository = _roomPicRepository;
            roomRepository = _roomRepository;
            hostelRepository = _hostelRepository;
        }


        [BindProperty]
        public Room Room { get; set; }
        //public int HostelId { get; set; }
        [Required(ErrorMessage = "Please chose at least one file.")]
        [DataType(DataType.Upload)]
        //[FileExtensions(Extensions = "png,jpg,jpeg,gif")]
        [Display(Name = "Choose Room images(s)")]
        [BindProperty]
        public IFormFile[] FileUploads { get; set; }
        public RoomPic RoomPic { get; set; }
        public Hostel hostel { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            HttpContext.Session.SetInt32("HostelId", id);
            hostel = await hostelRepository.GetHostelByID(id);
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
            if (FileUploads != null)
            {
                foreach (var FileUpload in FileUploads)
                {
                    RoomPic roomPic = new RoomPic
                    {
                        RoomId = Room.RoomId,
                        RoomPicUrl = await Utilities.UploadFile(FileUpload, @"images\rooms\", FileUpload.FileName)
                    };
                    await roomPicRepository.AddRoomPic(roomPic);
                }
            }
            return RedirectToPage("../Hostels/Edit", new { id = Room.HostelId });
        }
    }
}
