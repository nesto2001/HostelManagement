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

namespace HostelManagement.Pages.Rents
{
    public class CreateModel : PageModel
    {
        private IAccountRepository accountRepository { get; }
        private IRentRepository rentRepository { get; }
        private IRoomRepository roomRepository { get; }
        private IRoomMemberRepository roomMemberRepository { get; }
        public CreateModel(IAccountRepository _accountRepository, IRentRepository _rentRepository,
                            IRoomRepository _roomRepository, IRoomMemberRepository _roomMemberRepository)
        {
            accountRepository = _accountRepository;
            rentRepository = _rentRepository;
            roomRepository = _roomRepository;
            roomMemberRepository = _roomMemberRepository;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            int UId;
            var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId != null)
            {
               UId = Int32.Parse(userId);
               account = await accountRepository.GetAccountByID(UId);
            }
            room = await roomRepository.GetRoomByID(id);
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
        public string message { get; set; }

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
            Rent.EndRentDate = Rent.StartRentDate.AddMonths(MonthRent);
            Rent.IsDeposited = 0;
            Rent.Status = 0;

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
            
            await rentRepository.AddRent(Rent);
            foreach (var RoomMem in RoomMember)
            { 
                if (!String.IsNullOrEmpty(RoomMem.UserEmail))
                {
                    RoomMem.RentId = Rent.RentId;
                    RoomMem.RoomId = Rent.RoomId;
                    RoomMem.StartRentDate = Rent.StartRentDate;
                    RoomMem.EndRentDate = Rent.EndRentDate;
                    RoomMem.IsPresentator = true;
                    RoomMem.Status = 1;
                    await roomMemberRepository.AddRoomMember(RoomMem);
                }
            }
            return RedirectToPage("./Index");
        }
    }
}
