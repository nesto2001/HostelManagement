using System.Collections.Generic;
using BusinessObject.BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HostelManagement.Pages.Hostels
{
    public class ViewModel : PageModel
    {
        public IEnumerable<Room> Rooms { get; set; }
        public Hostel Hostel { get; set; }
        public void OnGet()
        {
        }
    }
}
