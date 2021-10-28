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
    public class Login : PageModel
    {
        private readonly IStringLocalizer<AccountResource> _localizer;
        private readonly ILogger<Login> _logger;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;

        public Login(
            SignInManager<AppUser> signInManager,
            UserManager<AppUser> userManager,
            ILogger<Login> logger,
            IStringLocalizer<AccountResource> localizer
        )
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _localizer = localizer;
        }

        [BindProperty]
        public LoginInputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalSchemes { get; set; }

        public string ReturnUrl { get; set; }

        public async Task OnGetAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalSchemes = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalSchemes = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (!ModelState.IsValid) return Page();

            await ReplaceEmailToUsernameOfInputIfNeeds();
            var result = await _signInManager.PasswordSignInAsync(
                Input.UsernameOrEmailAddress,
                Input.Password,
                Input.RememberMe,
                true);

            if (result.Succeeded)
            {
                return Redirect(returnUrl);
            }

            // TODO: add 2fa logic

            if (result.IsLockedOut)
            {
                var message = _localizer["The user is locked out, re-try in 5 minutes"];
                ModelState.AddModelError(string.Empty, message);
                return Page();
            }

            if (result.IsNotAllowed)
            {
                var message = _localizer["The user is not allowed to log in"];
                ModelState.AddModelError(string.Empty, message);
                return Page();
            }

            // wrong username or password
            ModelState.AddModelError(string.Empty, _localizer["Your credential is invalid"]);
            return Page();
        }

        public async Task<IActionResult> OnGetCancel(string returnUrl = null)
        {
            ExternalSchemes = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            ModelState.AddModelError(string.Empty, "Cancel will be implemented in IdentityServer");

            return Page();
        }

        private async Task ReplaceEmailToUsernameOfInputIfNeeds()
        {
            var userByUsername = await _userManager.FindByNameAsync(Input.UsernameOrEmailAddress);
            if (userByUsername != null) return;

            var userByEmail = await _userManager.FindByEmailAsync(Input.UsernameOrEmailAddress);
            if (userByEmail == null) return;

            Input.UsernameOrEmailAddress = userByEmail.UserName;
        }
    }
}