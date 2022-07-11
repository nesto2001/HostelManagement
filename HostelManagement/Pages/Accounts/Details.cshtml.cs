using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject.BusinessObject;
using DataAccess.Repository;
using DataAccess;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HostelManagement.Pages.Accounts
{
    public class DetailsModel : PageModel
    {
        private readonly DataAccess.HostelManagementContext _context;
        private IIdentityCardRepository _identityCardRepository;
        private IAccountRepository _accountRepository;

        public DetailsModel(DataAccess.HostelManagementContext context, IIdentityCardRepository identityCardRepository, IAccountRepository accountRepository)
        {
            _context = context;
            _identityCardRepository = identityCardRepository;
            _accountRepository = accountRepository;
        }

        public Account Account { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Account = await _accountRepository.GetAccountByID(id.Value);

            ViewData["IdCardNumber"] = new SelectList(_context.IdentityCards, "IdCardNumber", "IdCardNumber");
            ViewData["FrontPic"] = new SelectList(_context.IdentityCards, "FrontIdPicUrl", "FrontIdPicUrl");
            ViewData["BackPic"] = new SelectList(_context.IdentityCards, "BackIdPicUrl", "BackIdPicUrl");

            if (Account == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
