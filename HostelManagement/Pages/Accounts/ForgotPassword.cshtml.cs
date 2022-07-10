using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessObject.BusinessObject;
using DataAccess.Repository;
using HostelManagement.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HostelManagement.Pages.Accounts
{
    public class ForgotPasswordModel : PageModel
    {
        private IAccountRepository accountRepository { get; }
        private ISendMailService sendMailService { get; }
        public ForgotPasswordModel(IAccountRepository _accountRepository, ISendMailService _sendMailService)
        {
            accountRepository = _accountRepository;
            sendMailService = _sendMailService;
        }

        [BindProperty]
        public Account Account { get; set; }
        public string message { get; set; }

        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {

            var acc = await accountRepository.GetAccountByEmail(Account.UserEmail);
            if (acc == null)
            {
                message = "Your email is not exist in system.";
                return Page();
            }
            else
            {
                Random random = new Random();
                string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                string pass = new string(Enumerable.Repeat(chars, 8)
                                                    .Select(s => s[random.Next(s.Length)]).ToArray());
                acc.UserPassword = pass;
                await accountRepository.UpdateAccount(acc);
                string body = "Dear, \n" +
                    "Your new password is " + pass + "\n" +
                    "Please login with this pass then change your password. " + "\n" +
                    "Thank you. \n" +
                    "Please send your feedback by reply mail.\n" +
                    "Best Regard,";
                await sendMailService.SendEmailAsync(acc.UserEmail, "Reset password", body);
                message = "Please check your mail and Login with new password";
            }
            return Page();
        }
    }
}
