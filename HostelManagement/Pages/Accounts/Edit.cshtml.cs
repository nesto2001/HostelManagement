using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObject.BusinessObject;
using DataAccess;
using Microsoft.AspNetCore.Http;
using HostelManagement.Helpers;
using DataAccess.Repository;

namespace HostelManagement.Pages.Accounts
{
    public class EditModel : PageModel
    {
        private readonly DataAccess.HostelManagementContext _context;
       

        public EditModel(DataAccess.HostelManagementContext context)
        {
            _context = context;
           
        }

        [BindProperty]
        public Account Account { get; set; }
        public String RoleName { get; set; }
        [BindProperty]
        public IFormFile[] FileUploads { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Account = await _context.Accounts
                .Include(a => a.IdCardNumberNavigation).FirstOrDefaultAsync(m => m.UserId == id);

            if (Account == null)
            {
                return NotFound();
            }
            

            /*ViewData["IdCardNumber"] = new SelectList(_context.IdentityCards, "IdCardNumber", "IdCardNumber");*/
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if(Request.Form["role"] == "")
            {
                
                Account.RoleName =  Account.RoleName;
            }
            else
            {
                Account.RoleName = Request.Form["role"];
            }
           
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
            else
            {
                Account.ProfilePicUrl = Account.ProfilePicUrl;
            }
            _context.Attach(Account).State = EntityState.Modified;
            

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountExists(Account.UserId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool AccountExists(int id)
        {
            return _context.Accounts.Any(e => e.UserId == id);
        }
    }
}
