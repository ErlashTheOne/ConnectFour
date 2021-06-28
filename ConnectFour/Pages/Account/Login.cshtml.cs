using ConnectFour.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ConnectFour.Data;
using Microsoft.Extensions.Logging;

namespace ConnectFour.Pages.Account
{
    [AllowAnonymous]
    
    public class LoginModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(
            ApplicationDbContext context,
            ILogger<LoginModel> logger
            )
        {
            _context = context;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {

            [Required]
            [Display(Name = "PlayerEmail")]
            [StringLength(32, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            public string PlayerEmail { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [StringLength(32, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 8)]
            public string PlayerPassword { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        //public async Task OnGetAsync(string returnUrl = null)
        //{
        //    // Clear the existing external cookie
        //    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        //    returnUrl ??= Url.Content("~/");

        //    ReturnUrl = returnUrl;
        //}

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;

            if (ModelState.IsValid)
            {
                var user = await AuthenticateUser(Input.PlayerEmail, Input.PlayerPassword);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }

                var claims = new List<Claim>
                {
                 new Claim(ClaimTypes.Name, user.PlayerEmail)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                 CookieAuthenticationDefaults.AuthenticationScheme,
                 new ClaimsPrincipal(claimsIdentity),
                 new AuthenticationProperties
                 {
                        IsPersistent = Input.RememberMe
                    });

                if (!Url.IsLocalUrl(returnUrl))
                {
                    returnUrl = Url.Content("~/");
                }

                return LocalRedirect(returnUrl);

            }

            // Something failed. Redisplay the form.
            return Page();
        }
        private async Task<Player> AuthenticateUser(string login, string password)
        {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                return null;
            }

            // For demonstration purposes, authenticate a user
            // with a static login name and password.
            // Assume that checking the database takes 500ms

            await Task.Delay(500);
            Player p = _context.Player.Where(x => x.PlayerEmail == login && x.PlayerPassword == password).FirstOrDefault();
            if(p == null)
            {
                return null;
            }

            //if (login.ToUpper() != "ADMINISTRATOR" || password != "P@ssw0rd")
            //{
            //    return null;
            //}

            return p; //{ PlayerEmail = "Administrator" };
        }
    }

}
