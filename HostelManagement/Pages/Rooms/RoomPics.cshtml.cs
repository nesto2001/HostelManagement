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
    [Authorize(Roles = "Owner")]
    public class RoomPicsModel : PageModel
    {
        private IRoomPicRepository roomPicRepository;

        public RoomPicsModel(IRoomPicRepository _roomPicRepository)
        {
            roomPicRepository = _roomPicRepository;
        }
        public IEnumerable<RoomPic> RoomPics { get; set; }
        public int RoomID { get; set; }
        [Required(ErrorMessage = "Please chose at least one file.")]
        [DataType(DataType.Upload)]
        //[FileExtensions(Extensions = "png,jpg,jpeg,gif")]
        [Display(Name = "Choose Room images(s)")]
        [BindProperty]
        public IFormFile[] FileUploads { get; set; }
        public async Task OnGetAsync(int id)
        {
            HttpContext.Session.SetInt32("RoomID", id);
            RoomID = (int)HttpContext.Session.GetInt32("RoomID");
            RoomPics = await roomPicRepository.GetRoomPicsOfARoom(id);
        }

        public async Task<IActionResult> OnGetDelete(int id)
        {
            RoomID = (int)HttpContext.Session.GetInt32("RoomID");
            RoomPics = await roomPicRepository.GetRoomPicsOfARoom(RoomID);
            RoomPic roomPic = RoomPics.Where(h => h.RoomPicsId == id).FirstOrDefault();
            await roomPicRepository.DeleteRoomPic(roomPic);
            return RedirectToPage("RoomPics", new {id = RoomPics.First().RoomId});
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                
                return Page();
            }
            RoomPic rPic = new RoomPic();
            if (FileUploads != null)
            {
                RoomID = (int)HttpContext.Session.GetInt32("RoomID");
                rPic.RoomId = RoomID;
                foreach (var FileUpload in FileUploads)
                {
                    rPic.RoomPicUrl = await Utilities.UploadFile(FileUpload, @"images\rooms\", FileUpload.FileName);
                    await roomPicRepository.AddRoomPic(rPic);
                }
            }
            return RedirectToPage("./Details", new { id = RoomID });
        }

    }
}
