using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Atomic.UnifiedAuth.Localization;
using Atomic.UnifiedAuth.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Atomic.UnifiedAuth.Pages.Account
{
    public class Register : PageModel
    {
        private readonly IStringLocalizer<AccountResource> _localizer;
        private readonly ILogger<Register> _logger;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;

        public Register(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ILogger<Register> logger,
            IStringLocalizer<AccountResource> localizer
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _localizer = localizer;
        }

        [BindProperty]
        public RegisterInputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalSchemes { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool IsExternal { get; set; }

        public async Task<IActionResult> OnGetAsync(string username, string emailAddress, string returnUrl = null)
        {
            Input = new RegisterInputModel
            {
                Username = username,
                EmailAddress = emailAddress,
            };

            ReturnUrl = returnUrl ?? Url.Content("~/");
            ExternalSchemes = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalSchemes = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (!ModelState.IsValid) return Page();

            var user = new AppUser { UserName = Input.Username, Email = Input.EmailAddress };
            var result = await _userManager.CreateAsync(user, Input.Password);
            if (result.Succeeded)
            {
                if (IsExternal)
                {
                    var loginInfo = await _signInManager.GetExternalLoginInfoAsync();
                    if (loginInfo == null)
                    {
                        const string message = "Error loading external login information";
                        _logger.LogWarning(message);
                        ModelState.AddModelError("ExternalLogin", _localizer[message]);

                        return Page();
                    }

                    result = await _userManager.AddLoginAsync(user, loginInfo);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User created an account using {Name} provider.",
                            loginInfo.LoginProvider);
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }

                        return Page();
                    }
                }
                else
                {
                    _logger.LogInformation("User created a new account with password.");
                }

                await _signInManager.SignInAsync(user, false);
                return LocalRedirect(returnUrl);
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("Register", _localizer[error.Description]);
            }

            return Page();
        }
    }
}