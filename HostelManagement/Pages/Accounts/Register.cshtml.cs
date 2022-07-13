using BusinessObject.BusinessObject;
using DataAccess.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using HostelManagement.Helpers;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;

namespace HostelManagement.Pages.Accounts
{
    public class RegisterModel : PageModel
    {
        private readonly DataAccess.HostelManagementContext context;
        private IAccountRepository accountRepository { get; }
        private IIdentityCardRepository identityCardRepository { get; }
        public RegisterModel(DataAccess.HostelManagementContext _context, IAccountRepository _accountRepository, IIdentityCardRepository _identityCardRepository)
        {
            context = _context;
            accountRepository = _accountRepository;
            identityCardRepository = _identityCardRepository;
        }

        [BindProperty]
        public InputModel Input { get; set; }
        public string MessageExistEmail { get; set; }
        public string MessageExistId { get; set; }
        [BindProperty]
        public IList<Account> Accounts { get; set; }
        [BindProperty]
        public IdentityCard IdCard { get; set; }
        [BindProperty]
        public IFormFile FrontPicUrl { get; set; }
        [BindProperty]
        public IFormFile BackPicUrl { get; set; }
        public string MessageDob { get; set; }
        public class InputModel : Account
        {
            [Required]
            [StringLength(50, ErrorMessage = "The {0} must be {2} to {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Confirm Password")]
            [Compare(nameof(UserPassword), ErrorMessage = "Did not match with password")]
            public string ConfirmPassword { get; set; }
        }
        public void OnGet()
        {
            //fixed
            //if (HttpContext.Session.GetInt32("isLoggedIn") == 1)
            //{
            //    Response.Redirect("/index");
            //}
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            else if (CheckExist(Input.UserEmail))
            {
                MessageExistEmail = "Email is existing. Please choose other email.";
                return Page();
            }
            else if (IdExists(Input.IdCardNumber))
            {
                MessageExistId = "ID is existing. Please choose other ID.";
                return Page();
            }
            else if (!CheckDob(Input.Dob))
            {
                MessageDob = "Invalid DOB. Your age must be 16 or greater.";
                return Page();
            }
            else
            {
                Account acc = null;
                IdCard.IdCardNumber = Input.IdCardNumber;
                if (FrontPicUrl != null && BackPicUrl != null)
                {
                    IdCard.FrontIdPicUrl = await Utilities.UploadFile(FrontPicUrl, @"images\accounts\idCard", FrontPicUrl.FileName);
                    IdCard.BackIdPicUrl = await Utilities.UploadFile(BackPicUrl, @"images\accounts\idCard", BackPicUrl.FileName);
                }
                await identityCardRepository.AddIdCard(IdCard);
                var account = new Account()
                {

                    UserEmail = Input.UserEmail,
                    FullName = Input.FullName,
                    UserPassword = Input.UserPassword,
                    PhoneNumber = Input.PhoneNumber,
                    RoleName = "renter",
                    Status = 1,
                    Dob = Input.Dob,
                    IdCardNumber = Input.IdCardNumber,
                    IdCardNumberNavigation = IdCard
                };

                
                await accountRepository.AddAccount(account);

                acc = accountRepository.GetAccountByEmail(account.UserEmail).Result;
                HttpContext.Session.SetInt32("isLoggedIn", 1);
                HttpContext.Session.SetInt32("ID", acc.UserId);
                HttpContext.Session.SetString("ContactName", acc.FullName);
                string success = $"Email {acc.UserEmail} register successfully.";
                HttpContext.Session.SetString("RegisterSuccess", success);
                return RedirectToPage("./Login");
            }
        }
        public bool CheckExist(string email)
        {
            Task<Account> acc = accountRepository.GetAccountByEmail(email);
            if (acc.Result != null) return true;
            else return false;
        }
        private bool IdExists(string id)
        {
            Task<IdentityCard> idCard = identityCardRepository.GetIdentityCardByID(id);
            if (idCard.Result != null) return true;
            else return false;
        }
        public bool CheckDob(DateTime? Dob)
        {
            TimeSpan timeDifference = DateTime.Now - Dob.Value;
            double Age = timeDifference.TotalDays / 365.2425;
            if (Age >= 16 && Age <=100) return true;
            else return false;
        }
    }
}
