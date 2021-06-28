using ConnectFour.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using ConnectFour.Data;
using Microsoft.Extensions.Logging;

namespace ConnectFour.Pages.Account
{
    [AllowAnonymous]
    public class LogoutModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<LoginModel> _logger;

        public LogoutModel(
            ApplicationDbContext context,
            ILogger<LoginModel> logger)
        {
            _context = context;
            _logger = logger;
        }


        public IActionResult OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                // Redirect to home page if the user is authenticated.
                return RedirectToPage("/Index");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToPage("/Account/Logout");
        }
    }
}
