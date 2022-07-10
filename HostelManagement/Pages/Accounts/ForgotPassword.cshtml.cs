using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessObject.BusinessObject;
using DataAccess.Repository;
using HostelManagement.Helpers;
using Microsoft.AspNetCore.Http;
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
        [BindProperty]
        public string Code { get; set; }
        public string message { get; set; }
        public string messageCode { get; set; }

        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostChangeAsync()
        {
            var acc = await accountRepository.GetAccountByEmail(Account.UserEmail);
            if (acc == null)
            {
                message = "Your email is not exist in system.";
                return Page();
            }
            else if (Code != HttpContext.Session.GetString("CODE"))
            {
                message = "CODE IS WRONG.";
                messageCode = "Retest";
                return Page();
            }
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
                messageCode = "DONE";
            }
            return Page();
        }

        public async Task<IActionResult> OnPostSendCodeAsync()
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
                string codeGen = new string(Enumerable.Repeat(chars, 8)
                                                    .Select(s => s[random.Next(s.Length)]).ToArray());
                string body = "Dear, \n" +
                    "CODE : " + codeGen + "\n" +
                    "Please enter this code to change your password. " + "\n" +
                    "Thank you. \n" +
                    "Please send your feedback by reply mail.\n" +
                    "Best Regard,";
                await sendMailService.SendEmailAsync(acc.UserEmail, "Send CODE to Reset password", body);
                HttpContext.Session.SetString("CODE", codeGen);
                message = "Please check your mail to get CODE";
                messageCode = "Sent";
            }
            return Page();
        }
    }
}
