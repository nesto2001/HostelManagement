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
using DataAccess.Repository;

namespace HostelManagement.Pages.Accounts
{
    public class CreateModel : PageModel
    {
        private readonly DataAccess.HostelManagementContext _context;
        private IAccountRepository _accountRepository;

        public CreateModel(DataAccess.HostelManagementContext context, IAccountRepository accountRepository)
        {
            _context = context;
            _accountRepository = accountRepository;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Account Account { get; set; }
        [BindProperty]
        public IFormFile FileUploads { get; set; }
        public string MessageExistEmail { get; set; }
        public string MessageDob { get; set; }
        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            else if (CheckExist(Account.UserEmail))
            {
                MessageExistEmail = "Email is existing. Please choose other email.";
                return Page();
            }
            else if (!CheckDob(Account.Dob))
            {
                MessageDob = "Invalid DOB.";
                return Page();
            }
            else
            {
                Account.RoleName = "renter";
                Account.Status = 1;
                if (FileUploads != null)
                {
                    Account.ProfilePicUrl = await Utilities.UploadFile(FileUploads, @"images\accounts", FileUploads.FileName);
                }
                await _accountRepository.AddAccount(Account);
                return RedirectToPage("./Index");
            }
        }
        public bool CheckExist(string email)
        {
            Task<Account> acc = _accountRepository.GetAccountByEmail(email);
            if (acc.Result != null) return true;
            else return false;
        }
        public bool CheckDob(DateTime? Dob)
        {
            TimeSpan timeDifference = DateTime.Now - Dob.Value;
            double Age = timeDifference.TotalDays / 365.2425;
            if (Age >= 16) return true;
            else return false;
        }
    }
}
