using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject.BusinessObject;
using DataAccess.Repository;

namespace HostelManagement.Pages.Accounts
{
    public class DeleteModel : PageModel
    {
        private readonly DataAccess.HostelManagementContext _context;
        private IAccountRepository _accountRepository;
        public DeleteModel(DataAccess.HostelManagementContext context, IAccountRepository accountRepository)
        {
            _context = context;
            _accountRepository = accountRepository;
        }

        [BindProperty]
        public Account Account { get; set; }

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
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Account = await _accountRepository.GetAccountByID(id.Value);

            if (Account != null)
            {
                //_context.Accounts.Remove(Account);
                //await _context.SaveChangesAsync();
                Account.Status = 0;
                await _accountRepository.UpdateAccount(Account);
            }

            return RedirectToPage("./Index");
        }
    }
}
