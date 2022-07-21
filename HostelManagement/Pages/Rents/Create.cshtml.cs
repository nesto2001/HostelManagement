using BusinessObject.BusinessObject;
using DataAccess.Repository;
using HostelManagement.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HostelManagement.Pages.Rents
{
    [Authorize(Roles = "Renter")]
    public class CreateModel : PageModel
    {
        private IAccountRepository accountRepository { get; }
        private IRentRepository rentRepository { get; }
        private IRoomRepository roomRepository { get; }
        private IHostelRepository hostelRepository { get; }
        private IRoomMemberRepository roomMemberRepository { get; }
        private ISendMailService sendMailService { get; }
        public CreateModel(IAccountRepository _accountRepository, IRentRepository _rentRepository,
                            IRoomRepository _roomRepository, IRoomMemberRepository _roomMemberRepository,
                            ISendMailService _sendMailService, IHostelRepository _hostelRepository)
        {
            accountRepository = _accountRepository;
            rentRepository = _rentRepository;
            roomRepository = _roomRepository;
            roomMemberRepository = _roomMemberRepository;
            sendMailService = _sendMailService;
            hostelRepository = _hostelRepository;
        }

        public async Task<IActionResult> OnGetAsync(int? id, int? extend)
        {
            if (id == null)
            {
                return RedirectToPage("../AccessDenied");
            }

            ViewData["extend"] = false;
            if (extend != null)
            {
                ViewData["extend"] = true;
                var rent = await rentRepository.GetRentByID((int)extend);
                StartRentDate = rent.EndRentDate.AddDays(1);
                HttpContext.Session.SetString("extend", StartRentDate.ToString());
            }
            int UId = 0;
            var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId != null)
            {
                UId = Int32.Parse(userId);
                account = await accountRepository.GetAccountByID(UId);
            }
            room = await roomRepository.GetRoomByID((int)id);
            if (room.Status != 1)
            {
                HttpContext.Session.SetString("AccessDeniedMessage", "This room is not available.");
                return RedirectToPage("../AccessDenied");
            }
            if (room.Hostel.HostelOwnerEmailNavigation.UserId == UId)
            {
                HttpContext.Session.SetString("AccessDeniedMessage", "You must not rent the room which the owner is you.");
                return RedirectToPage("../AccessDenied");
            }
            hostel = await hostelRepository.GetHostelByID(room.HostelId);
            ViewData["RentedBy"] = account.UserEmail;
            ViewData["RoomId"] = id;
            return Page();
        }

        [BindProperty]
        public Rent Rent { get; set; }
        [BindProperty]
        [Required]
        [Range(1, 24, ErrorMessage = "Your contract must be from 1 to 24 months.")]
        public int MonthRent { get; set; }
        [BindProperty]
        public RoomMember[] RoomMember { get; set; }
        public Account account { get; set; }
        [BindProperty]
        public Room room { get; set; }
        public Hostel hostel { get; set; }
        public string message { get; set; }
        [BindProperty]
        public DateTime StartRentDate { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                int UId = Int32.Parse(userId);
                account = await accountRepository.GetAccountByID(UId);
                room = await roomRepository.GetRoomByID(Rent.RoomId);
                hostel = await hostelRepository.GetHostelByID(room.HostelId);
                ViewData["RentedBy"] = account.UserEmail;
                ViewData["RoomId"] = room.RoomId;
                return Page();
            }
            if (HttpContext.Session.GetString("extend") != null)
            {
                Rent.StartRentDate = DateTime.Parse(HttpContext.Session.GetString("extend"));
                Rent.IsDeposited = 2;
                Rent.Status = 1;
            }
            else
            {
                Rent.IsDeposited = 0;
                Rent.Status = 0;
            }
            Rent.EndRentDate = Rent.StartRentDate.AddMonths(MonthRent);


            IEnumerable<Rent> rents = await rentRepository.GetRentListByRoom(Rent.RoomId);
            Rent re = rents.FirstOrDefault(r => r.EndRentDate.AddDays(5).Date >= Rent.StartRentDate.Date && r.Status == 3);
            bool check = false;
            if (re != null && HttpContext.Session.GetString("extend") == null)
            {
                if (re.EndRentDate.AddDays(5).Date >= Rent.StartRentDate.Date)
                {
                    message += $"This room already exist an contract to {re.EndRentDate.ToString("dd/MM/yyyy")}. (+5 days to clean room)\n";
                    check = true;
                }
                if (check)
                {
                    message += $"So you can't rent this room from {Rent.StartRentDate.ToString("dd/MM/yyyy")} to {Rent.EndRentDate.ToString("dd/MM/yyyy")}";
                    var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                    int UId = Int32.Parse(userId);
                    account = await accountRepository.GetAccountByID(UId);
                    room = await roomRepository.GetRoomByID(Rent.RoomId);
                    hostel = await hostelRepository.GetHostelByID(room.HostelId);
                    ViewData["RentedBy"] = account.UserEmail;
                    ViewData["RoomId"] = room.RoomId;
                    return Page();
                }
            }
            Rent rentCur = rents.FirstOrDefault(r => r.Status == 1);
            if (rentCur != null)
            {
                rentCur.Status = 5;
                await rentRepository.UpdateRent(rentCur);
            }
            await rentRepository.AddRent(Rent);

            int countCurrent = 0;
            foreach (var RoomMem in RoomMember)
            {

                if (!String.IsNullOrEmpty(RoomMem.UserEmail))
                {
                    RoomMem.RentId = Rent.RentId;
                    RoomMem.RoomId = Rent.RoomId;
                    RoomMem.StartRentDate = Rent.StartRentDate;
                    RoomMem.EndRentDate = Rent.EndRentDate;
                    RoomMem.IsPresentator = false;
                    if (countCurrent == 0) RoomMem.IsPresentator = true;
                    RoomMem.Status = 1;
                    await roomMemberRepository.AddRoomMember(RoomMem);
                    countCurrent++;
                }
            }
            room = await roomRepository.GetRoomByID(Rent.RoomId);
            string body = "New contract is created";
            if (rentCur != null)
            {
                body = "Dear, \n" +
                    "Your rent number is " + Rent.RentId + "\n" +
                    "Thank you. \n" +
                    "Please send your feedback by reply mail.\n" +
                    "Best Regard,";
            }
            else
            {
                body = "Dear, \n" +
                    "Your rent number is " + Rent.RentId + "\n" +
                    "Please pay deposit before 24h of start rent time. (" + Rent.StartRentDate +
                    "). Thank you. \n" +
                    "Please send your feedback by reply mail.\n" +
                    "Best Regard,";
            }
            await sendMailService.SendEmailAsync(Rent.RentedBy, "New contract", body);
            room.RoomCurrentCapacity = countCurrent;
            room.Status = 4;
            await roomRepository.UpdateRoom(room);
            HttpContext.Session.Remove("extend");
            return RedirectToPage("./Details", new { id = Rent.RentId });
        }
    }
}
