using BusinessObject.BusinessObject;
using DataAccess.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HostelManagement.Pages.Hostels
{
    public class ViewModel : PageModel
    {

        private IHostelRepository _hostelRepository;
        private IRoomRepository _roomRepository;

        public ViewModel(IHostelRepository hostelRepository, IRoomRepository roomRepository)
        {
            _hostelRepository = hostelRepository;
            _roomRepository = roomRepository;
        }

        public Hostel Hostel { get; set; }
        public IEnumerable<Room> Rooms { get; set; }
        public string UserRole { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (HttpContext.User.Claims != null)
            {
                var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                //int UId = Int32.Parse(userId);
                UserRole = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
            }
            HttpContext.Session.SetInt32("HostelID", (int)id);
            Hostel = await _hostelRepository.GetHostelByID((int)id);
            Rooms = await _roomRepository.GetRoomsOfAHostel((int)id);
            Rooms = Rooms.Where(room => room.Status == 1);
            if (Hostel == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
