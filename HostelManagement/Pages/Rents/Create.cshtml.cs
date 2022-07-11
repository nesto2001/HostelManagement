using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObject.BusinessObject;
using DataAccess;
using DataAccess.Repository;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using HostelManagement.Helpers;

namespace HostelManagement.Pages.Rents
{
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
            int UId;
            var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId != null)
            {
               UId = Int32.Parse(userId);
               account = await accountRepository.GetAccountByID(UId);
            }
            room = await roomRepository.GetRoomByID((int)id);
            hostel = await hostelRepository.GetHostelByID(room.HostelId);
            ViewData["RentedBy"] = account.UserEmail;
            ViewData["RoomId"] = id;
            return Page();
        }

        [BindProperty]
        public Rent Rent { get; set; }
        [BindProperty]
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
                ViewData["RentedBy"] = account.UserEmail;
                ViewData["RoomId"] = room.RoomId;
                return Page();
            }
            if (HttpContext.Session.GetString("extend") != null)
            {
                Rent.StartRentDate = DateTime.Parse(HttpContext.Session.GetString("extend"));
                Rent.IsDeposited = 2;
                Rent.Status = 1;
                HttpContext.Session.Remove("extend");
            } else
            {
                Rent.IsDeposited = 0;
                Rent.Status = 0;
            }
            Rent.EndRentDate = Rent.StartRentDate.AddMonths(MonthRent);
            

            IEnumerable<Rent> rents = await rentRepository.GetRentListByRoom(Rent.RoomId);
            Rent re = rents.FirstOrDefault(r => r.Status == 2);
            bool check = false;
            if (re != null)
            {
                if (re.EndRentDate.AddDays(5).Date >= Rent.StartRentDate.Date)
                {
                    message += $"This room already exist an contract to {re.EndRentDate.ToString("dd/MM/yyyy")}. \n";
                    check = true;
                }
                if (check)
                {
                    message += $"So you can't rent this room from {Rent.StartRentDate.ToString("dd/MM/yyyy")} to {Rent.EndRentDate.ToString("dd/MM/yyyy")}";
                    var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                    int UId = Int32.Parse(userId);
                    account = await accountRepository.GetAccountByID(UId);
                    room = await roomRepository.GetRoomByID(Rent.RoomId);
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
                    if(!(countCurrent>0)){
                    RoomMem.IsPresentator = true;
                    }
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
            } else
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
            return RedirectToPage("./Index");
        }
    }
}
