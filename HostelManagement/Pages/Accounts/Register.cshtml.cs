using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BusinessObject.BusinessObject;
using DataAccess.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace HostelManagement.Pages.Accounts
{
    public class RegisterModel : PageModel
    {
        private IAccountRepository accountRepository { get; }
        public RegisterModel(IAccountRepository _accountRepository)
        {
            accountRepository = _accountRepository;
        }

        [BindProperty]
        public InputModel Input { get; set; }
        public string MessageExistEmail { get; set; }

        public IList<Account> Accounts { get; set; }

        public class InputModel : Account
        {
            [Required]
            [StringLength(50, ErrorMessage = "The {0} must be {2} to {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Confirm Password")]
            [Compare(nameof(UserPassword), ErrorMessage = "Did not match with password")]
            public string ConfirmPassword { get; set; }
        }

        public bool CheckExist(string email)
        {
            Task<Account> acc = accountRepository.GetAccountByEmail(email);
            if (acc.Result != null) return true;
            else return false;
        }

        public void OnGet()
        {
            //if (HttpContext.Session.GetInt32("isLoggedIn") == 1)
            //{
            //    Response.Redirect("/index");
            //}
        }

        public async Task OnPost()
        {
            /*if (!ModelState.IsValid)
            {
            }
            else if (CheckExist(Input.UserEmail))
            {
                MessageExistEmail = "Email is existing. Please choose other email.";
            }
            else
            {
                Account acc = null;
                var account = new Account()
                {
                    CustomerId = b,
                    Email = Input.Email,
                    ContactName = Input.ContactName,
                    Password = Input.Password,
                    Phone = Input.Phone,
                    Address = Input.Address
                };
                await customerRepository.AddCustomer(customer);
                cus = customerRepository.GetCustomerByEmail(customer.Email).Result;
                HttpContext.Session.SetInt32("isLoggedIn", 1);
                HttpContext.Session.SetString("ID", cus.CustomerId);
                HttpContext.Session.SetString("ContactName", cus.ContactName);
                Response.Redirect("/Index");
            }*/
        }

    }
}
