using System.Threading.Tasks;
using Atomic.UnifiedAuth.Localization;
using Atomic.UnifiedAuth.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;

namespace Atomic.UnifiedAuth.Pages.Account
{
    public class Login : PageModel
    {
        private readonly IStringLocalizer<AccountResource> _localizer;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;

        public Login(
            SignInManager<AppUser> signInManager,
            UserManager<AppUser> userManager,
            IStringLocalizer<AccountResource> localizer
        )
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _localizer = localizer;
        }

        [BindProperty]
        public LoginInputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public string ErrorMessage { get; set; }

        public async Task OnGetAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
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
                ErrorMessage = _localizer["The user is locked out, re-try in 5 minutes"];
                return Page();
            }

            if (result.IsNotAllowed)
            {
                ErrorMessage = _localizer["The user is not allowed to log in"];
                return Page();
            }

            // wrong username or password
            ErrorMessage = _localizer["Your credential is invalid"];
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