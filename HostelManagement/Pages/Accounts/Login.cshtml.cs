using BusinessObject.BusinessObject;
using DataAccess.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HostelManagement.Pages.Accounts
{
    public class LoginModel : PageModel
    {
        private IAccountRepository accountRepository { get; }
        public LoginModel(IAccountRepository _accountRepository)
        {
            accountRepository = _accountRepository;
        }

        [BindProperty]
        public Account Account { get; set; }
        public string message { get; set; }

        public IList<Account> Accounts { get; set; }
        public async Task<IActionResult> OnPost()
        {

            Task<Account> acc = accountRepository.GetLoginAccount(Account.UserEmail, Account.UserPassword);
            if (acc.Result == null)
            {
                message = "Your account or password is incorrect. Try again!";
                return Page();
            }
            else if (acc.Result.Status != 1)
            {
                message = "Your account is locked!";
                return Page();
            }
            else if (acc.Result.RoleName.Equals("admin"))
            {
                var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier, acc.Result.UserId.ToString()),
                            new Claim(ClaimTypes.Role, "Admin"),
                            new Claim(ClaimTypes.Name, acc.Result.FullName),
                            new Claim(ClaimTypes.Email, acc.Result.UserEmail)
                        };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);
                //HttpContext.Session.SetInt32("isLoggedIn", 1);
                //HttpContext.Session.SetString("ContactName", "Admin");
                //HttpContext.Session.SetString("ID", "admin");
                return RedirectToPage("../AdminDashboard"); //return AdminDashboard

            }
            else if (acc.Result.RoleName.Equals("owner"))
            {
                var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier, acc.Result.UserId.ToString()),
                            new Claim(ClaimTypes.Role, "Renter"),
                            new Claim(ClaimTypes.Name, acc.Result.FullName),
                            new Claim(ClaimTypes.Email, acc.Result.UserEmail)
                        };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                //HttpContext.Session.SetInt32("isLoggedIn", 1);
                //HttpContext.Session.SetString("ID", cus.Result.CustomerId);
                //HttpContext.Session.SetString("ContactName", cus.Result.ContactName);
                var HostelView = HttpContext.Session.GetInt32("HostelID");
                if (HostelView != null)
                {
                    int rv = (int)HostelView;
                    return RedirectToPage("../Hostels/View", new { id = rv });
                }
                else
                {
                    return RedirectToPage("../Index");
                }
            }
            else if (acc.Result.RoleName.Equals("renter"))
            {
                var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier, acc.Result.UserId.ToString()),
                            new Claim(ClaimTypes.Role, "Renter"),
                            new Claim(ClaimTypes.Name, acc.Result.FullName),
                            new Claim(ClaimTypes.Email, acc.Result.UserEmail)
                        };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                //HttpContext.Session.SetInt32("isLoggedIn", 1);
                //HttpContext.Session.SetString("ID", cus.Result.CustomerId);
                //HttpContext.Session.SetString("ContactName", cus.Result.ContactName);
                var HostelView = HttpContext.Session.GetInt32("HostelID");
                if (HostelView != null)
                {
                    int rv = (int)HostelView;
                    return RedirectToPage("../Hostels/View", new { id = rv });
                }
                else
                {
                    return RedirectToPage("../Index");
                }

            }

            else
            {
                message = "Your account or password is incorrect. Try again!";
            }
            return Page();
        }

        public async Task<IActionResult> OnGetLogout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToPage("/Index");
        }

        public IActionResult OnGet()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Index");
            }
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("RegisterSuccess")))
            {
                ViewData["RegisterSuccessMessage"] = HttpContext.Session.GetString("RegisterSuccess");
                HttpContext.Session.Remove("RegisterSuccess");
            }
            return Page();
        }
    }
}
