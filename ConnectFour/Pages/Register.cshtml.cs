using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using ConnectFour.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using ConnectFour.Data;

namespace ConnectFour.Pages
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RegisterModel> _logger;

        public RegisterModel(
            ApplicationDbContext context,
            ILogger<RegisterModel> logger)
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
            [DataType(DataType.Text)]
            [Display(Name = "PlayerNickName")]
            public string PlayerNickName { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "PlayerProfilePic")]
            public string PlayerProfilePic { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "PlayerEmail")]
            public string PlayerEmail { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "PlayerPassword")]
            public string PlayerPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "ConfirmPassword")]
            [Compare("PlayerPassword", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }
        //public async Task OnGetAsync(string returnUrl = null)
        //{
        //    ReturnUrl = returnUrl;
        //    ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        //}

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            //ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            //if (ModelState.IsValid)
            //{
                var user = new Player
                {
                    PlayerNickName = Input.PlayerNickName,
                    PlayerProfilePic = Input.PlayerProfilePic,
                    PlayerPassword = Input.PlayerPassword,
                    PlayerEmail = Input.PlayerEmail

                };
                var result = await _context.Player.AddAsync(user);
                //if (result.Succeeded)
                //{
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("User created a new account with password.");
                    return RedirectToPage("./Index");
                //}
                //else
                //{
                //    await _signInManager.SignInAsync(user, isPersistent: false);
                //    return LocalRedirect(returnUrl);
                //}
                //foreach (var error in result.Errors)
                //{
                //    ModelState.AddModelError(string.Empty, error.Description);
                //}
                // If we got this far, something failed, redisplay form

            //}
            return Page();

        }

    }
}
