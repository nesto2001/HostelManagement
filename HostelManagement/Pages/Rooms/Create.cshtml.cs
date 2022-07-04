﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObject.BusinessObject;
using DataAccess;
using DataAccess.Repository;
using HostelManagement.Helpers;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace HostelManagement.Pages.Rooms
{
    public class CreateModel : PageModel
    {
        private IHostelRepository hostelRepository;
        private ILocationRepository locationRepository;
        private IHostelPicRepository hostelPicRepository;
        private IRoomRepository roomRepository;
        private IRoomPicRepository roomPicRepository;

        public CreateModel(IHostelRepository _hostelRepository,
            ILocationRepository _locationRepository, IHostelPicRepository _hostelPicRepository,
            IRoomRepository _roomRepository, IRoomPicRepository _roomPicRepository)
        {
            hostelRepository = _hostelRepository;
            locationRepository = _locationRepository;
            hostelPicRepository = _hostelPicRepository;
            roomRepository = _roomRepository;
            roomPicRepository = _roomPicRepository;
        }


        [BindProperty]
        public Room[] Rooms { get; set; }
        public int countPic { get; set; }
        public int countRoom { get; set; }
        [Required(ErrorMessage = "Please chose at least one file.")]
        [DataType(DataType.Upload)]
        //[FileExtensions(Extensions = "png,jpg,jpeg,gif")]
        [Display(Name = "Choose Room images(s)")]
        [BindProperty]
        public IFormFile[][] FileUploads { get; set; }

        public async Task<IActionResult> OnGetAsync(int countPics, int countRooms)
        {
            countPic = countPics;
            countRoom = countRooms;
            return Page();
        }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            Location location = SessionHelper.GetObjectFromJson<Location>(HttpContext.Session, "locationPending");
            if (location != null)
            {
                await locationRepository.AddLocation(location);
            }
            Hostel hostel = SessionHelper.GetObjectFromJson<Hostel>(HttpContext.Session, "hostelPending");
            if (hostel != null)
            {
                await hostelRepository.AddHostel(hostel);
            }
            for (int i = 1; i <= countPic; i++)
            {
                string key = $"hostelPicPending{i}";
                HostelPic hostelPic = SessionHelper.GetObjectFromJson<HostelPic>(HttpContext.Session,key);
                if (hostelPic != null)
                {
                    await hostelPicRepository.AddHostelPic(hostelPic);
                }
            }
            int a = 0;
            foreach (var Room in Rooms)
            {
                Room.HostelId = hostel.HostelId;
                await roomRepository.AddRoom(Room);
                if (FileUploads != null)
                {
                    foreach (var FileUpload in FileUploads[a])
                    {
                        RoomPic roomPic = new RoomPic
                        {
                            RoomId = Room.RoomId,
                            RoomPicUrl = await Utilities.UploadFile(FileUpload, @"images\rooms\", FileUpload.FileName)
                        };
                        await roomPicRepository.AddRoomPic(roomPic);
                    }
                }
                a++;
            }
            return RedirectToPage("../Hostels/Details", new {id=hostel.HostelId});
        }
    }
}
