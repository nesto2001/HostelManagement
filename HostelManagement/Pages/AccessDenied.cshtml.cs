using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HostelManagement.Pages
{
    public class AccessDeniedModel : PageModel
    {
        public void OnGet()
        {
            if (!String.IsNullOrEmpty(HttpContext.Session.GetString("AccessDeniedMessage")))
            {
                ViewData["AccessDeniedMessage"] = HttpContext.Session.GetString("AccessDeniedMessage");
                HttpContext.Session.Remove("AccessDeniedMessage");
            }
        }
    }
}
