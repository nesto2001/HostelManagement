using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessObject.BusinessObject;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HostelManagement.Pages.Accounts
{
    public class LoginModel : PageModel
    {
        private readonly DataAccess.Data.HostelManagementDBContext _context;

        public LoginModel(DataAccess.Data.HostelManagementDBContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Account Account { get; set; }

        public IList<Account> Accounts { get; set; }
        public IActionResult OnPost()
        {
            Accounts = _context.Accounts.ToList();
            if (CheckLogin(Account.UserEmail, Account.UserPassword))
            {
                HttpContext.Session.SetString("Email", Account.UserEmail);
                return RedirectToPage("Index");
            }
            else
            {
                return Page();
            }
        }

        public bool CheckLogin(String email, String password)
        {
            Account acc = _context.Accounts.SingleOrDefault(account => account.UserEmail == email && account.UserPassword == password);
            if (acc != null) return true;
            else return false;
        }
    }
}
