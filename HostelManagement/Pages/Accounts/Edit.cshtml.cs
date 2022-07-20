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
        private IAccountRepository _accountRepository { get; }
        private IIdentityCardRepository _identityCardRepository { get; }

        public EditModel(IAccountRepository accountRepository, IIdentityCardRepository identityCardRepository)
        {
            _accountRepository = accountRepository;
            _identityCardRepository = identityCardRepository;
        }

        [BindProperty]
        public Account Account { get; set; }
        public class InputModel : Account { }
        [BindProperty]
        public InputModel Input { get; set; }
        [BindProperty]
        public IdentityCard IdCard { get; set; }
        public IdentityCard IdCardNav { get; set; }
        [BindProperty]
        public IFormFile FileUploads { get; set; }
        [BindProperty]
        public IFormFile FrontPicUrl { get; set; }
        [BindProperty]
        public IFormFile BackPicUrl { get; set; }
        public string MessageExistId { get; set; }
        public string MessageExistEmail { get; set; }
        public string MessageDob { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Account = await _accountRepository.GetAccountByID(id.Value);
            if (Account.IdCardNumberNavigation != null)
            {
                var _acc = Account;
                _acc.IdCardNumberNavigation.Accounts = null;
                _acc.Hostels = null;
                _acc.Rents = null;
            }
            SessionHelper.SetObjectAsJson(HttpContext.Session, "AccountEdit", Account);
            
            IdCardNav = Account.IdCardNumberNavigation;
            if (Account == null)
            {
                return NotFound();
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            Account acc = SessionHelper.GetObjectFromJson<Account>(HttpContext.Session, "AccountEdit");

            if (!ModelState.IsValid)
            {
                return Page();
            }
            else if (IdExists(Input.IdCardNumber))
            {
                MessageExistId = "ID is existing. Please choose other ID.";
                return Page();
            }
            else if (!CheckDob(Account.Dob))
            {
                MessageDob = "Invalid DOB.";
                return Page();
            }
            else
            {
                if (acc?.IdCardNumber == null)
                {
                    IdCard.IdCardNumber = Account.IdCardNumber;
                    if (FrontPicUrl != null && BackPicUrl != null)
                    {
                        IdCard.FrontIdPicUrl = await Utilities.UploadFile(FrontPicUrl, @"images\accounts\idCard", FrontPicUrl.FileName);
                        IdCard.BackIdPicUrl = await Utilities.UploadFile(BackPicUrl, @"images\accounts\idCard", BackPicUrl.FileName);
                    }
                    else
                    {
                        IdCard.FrontIdPicUrl = acc.IdCardNumberNavigation.FrontIdPicUrl;
                        IdCard.BackIdPicUrl = acc.IdCardNumberNavigation.BackIdPicUrl;
                    }
                    await _identityCardRepository.AddIdCard(IdCard);
                }

                if (FileUploads != null)
                {
                    Account.ProfilePicUrl = await Utilities.UploadFile(FileUploads, @"images\accounts\", FileUploads.FileName);
                }
                else
                {
                    Account.ProfilePicUrl = acc.ProfilePicUrl;
                }
                Account.IdCardNumberNavigation = null;
                await _accountRepository.UpdateAccount(Account);

                return RedirectToPage("./Details", new { id= Account.UserId });
            }
        }
        public bool CheckExist(string email)
        {
            Task<Account> acc = _accountRepository.GetAccountByEmail(email);
            if (acc.Result != null)
            {
                if (acc.Result.UserId != Account.UserId)
                    return true;
                else return false;
            }
            else return false;
        }
        private bool IdExists(string id)
        {
            Task<IdentityCard> idCard = _identityCardRepository.GetIdentityCardByID(id);
            if (idCard.Result != null) return true;
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
