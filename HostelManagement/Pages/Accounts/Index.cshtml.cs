using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject.BusinessObject;
using DataAccess;
using Microsoft.AspNetCore.Authorization;
using DataAccess.Repository;
using System.Security.Claims;

namespace HostelManagement.Pages.Accounts
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly DataAccess.HostelManagementContext _context;
        private IAccountRepository _accountRepository;
        private IIdentityCardRepository _identityCardRepository;

        public IndexModel(DataAccess.HostelManagementContext context, IAccountRepository accountRepository, IIdentityCardRepository identityCardRepository)
        {
            _context = context;
            _accountRepository = accountRepository;
            _identityCardRepository = identityCardRepository;
        }

        public IEnumerable<Account> Account { get;set; }

        public async Task OnGetAsync(string searchUser)
        {
            Account = await _accountRepository.GetAccountList();
            IEnumerable<Account> AccountSearch = Account;
            if (!String.IsNullOrEmpty(searchUser))
            {
                Account = Account.Where(a => a.FullName.ToLower().Contains(searchUser.ToLower()) ||
                                            a.UserEmail.ToLower().Contains(searchUser.ToLower()))
                                        .ToList();
            }
            ViewData["searchUser"] = searchUser;
        }
        public async Task<IActionResult> OnPostDeactivate(int id)
        {
            await _accountRepository.InactivateUser(id);
            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnPostReactivate(int id)
        {
            await _accountRepository.ActivateUser(id);
            return RedirectToPage("./Index");
        }
    }
}
