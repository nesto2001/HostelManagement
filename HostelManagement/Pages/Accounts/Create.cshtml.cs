using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObject.BusinessObject;
using DataAccess;
using Microsoft.AspNetCore.Http;
using HostelManagement.Helpers;

namespace HostelManagement.Pages.Accounts
{
    public class CreateModel : PageModel
    {
        private readonly DataAccess.HostelManagementContext _context;

        public CreateModel(DataAccess.HostelManagementContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["IdCardNumber"] = new SelectList(_context.IdentityCards, "IdCardNumber", "IdCardNumber");
            return Page();
        }

        [BindProperty]
        public Account Account { get; set; }
        [BindProperty]
        public IFormFile[] FileUploads { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            Account.RoleName = Request.Form["role"];
            int countPic = 0;
            if (FileUploads != null)
            {

                int i = 1;
                countPic = FileUploads.Count();
                foreach (var FileUpload in FileUploads)
                {
                    Account.ProfilePicUrl = await Utilities.UploadFile(FileUpload, @"images\accounts\", FileUpload.FileName);

                    i++;
                }
            }
            _context.Accounts.Add(Account);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
