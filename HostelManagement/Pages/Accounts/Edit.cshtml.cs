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
        private IAccountRepository _accountRepository { get; }
        private IIdentityCardRepository _identityCardRepository { get; }

        public EditModel(DataAccess.HostelManagementContext context, IAccountRepository accountRepository, IIdentityCardRepository identityCardRepository)
        {
            _context = context;
            _accountRepository = accountRepository;
            _identityCardRepository = identityCardRepository;
        }

        [BindProperty]
        public Account Account { get; set; }
        [BindProperty]
        public IdentityCard IdCard { get; set; }
        public String RoleName { get; set; }
        [BindProperty]
        public IFormFile FileUploads { get; set; }
        [BindProperty]
        public IFormFile FrontPicUrl { get; set; }
        [BindProperty]
        public IFormFile BackPicUrl { get; set; }
        public string MessageExistEmail { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Account = await _accountRepository.GetAccountByID(id.Value);

            if (Account == null)
            {
                return NotFound();
            }

            ViewData["IdCardNumber"] = new SelectList(_context.IdentityCards, "IdCardNumber", "IdCardNumber");
            ViewData["FrontPic"] = new SelectList(_context.IdentityCards, "FrontIdPicUrl", "FrontPicUrl");
            ViewData["BackPic"] = new SelectList(_context.IdentityCards, "BackIdPicUrl", "BackPicUrl");
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

            IdCard.IdCardNumber = Account.IdCardNumber;
            if (FrontPicUrl != null && BackPicUrl != null)
            {
                IdCard.FrontIdPicUrl = await Utilities.UploadFile(FrontPicUrl, @"images\accounts\idCard", FrontPicUrl.FileName);
                IdCard.BackIdPicUrl = await Utilities.UploadFile(BackPicUrl, @"images\accounts\idCard", BackPicUrl.FileName);
            }
            else
            {
                IdCard.FrontIdPicUrl = IdCard.FrontIdPicUrl;
                IdCard.BackIdPicUrl = IdCard.BackIdPicUrl;
            }
            await _identityCardRepository.UpdateIdCard(IdCard);

            if (FileUploads != null)
            {
                 Account.ProfilePicUrl = await Utilities.UploadFile(FileUploads, @"images\accounts\", FileUploads.FileName);
            }
            else
            {
                Account.ProfilePicUrl = Account.ProfilePicUrl;
            }
            await _accountRepository.UpdateAccount(Account);

            return RedirectToPage("./Index");
        }
        public bool CheckExist(string email)
        {
            Task<Account> acc = _accountRepository.GetAccountByEmail(email);
            if (acc.Result != null) return true;
            else return false;
        }
        private bool IdExists(string id)
        {
            Task<IdentityCard> idCard = _identityCardRepository.GetIdentityCardByID(id);
            if (idCard.Result != null) return true;
            else return false;
        }
    }
}
