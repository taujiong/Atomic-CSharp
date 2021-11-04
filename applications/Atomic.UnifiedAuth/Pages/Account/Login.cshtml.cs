using System;
using System.Threading.Tasks;
using Atomic.UnifiedAuth.Localization;
using Atomic.UnifiedAuth.Models;
using Atomic.UnifiedAuth.Users;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Atomic.UnifiedAuth.Pages.Account
{
    public class LoginPageModel : AccountPageModel
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IStringLocalizer<AccountResource> _localizer;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;

        public LoginPageModel(
            SignInManager<AppUser> signInManager,
            UserManager<AppUser> userManager,
            IStringLocalizer<AccountResource> localizer,
            IIdentityServerInteractionService interaction
        )
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _localizer = localizer;
            _interaction = interaction;
        }

        [BindProperty]
        public LoginInputModel Input { get; set; }

        public async Task OnGetAsync()
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            var context = await _interaction.GetAuthorizationContextAsync(ReturnUrl);
            Input = new LoginInputModel
            {
                UsernameOrEmailAddress = context?.LoginHint,
            };
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var context = _interaction.GetAuthorizationContextAsync(ReturnUrl);

            await ReplaceEmailToUsernameOfInputIfNeeds();
            var result = await _signInManager.PasswordSignInAsync(
                Input.UsernameOrEmailAddress,
                Input.Password,
                Input.RememberMe,
                true);

            if (result.Succeeded)
            {
                if (context != null || Url.IsLocalUrl(ReturnUrl) || string.IsNullOrEmpty(ReturnUrl))
                    return RedirectSafely();

                throw new Exception(_localizer["Invalid return url."]);
            }

            // TODO: add 2fa logic

            if (result.IsLockedOut)
            {
                PageErrorMessage = _localizer["The user is locked out, re-try in 5 minutes"];
                return Page();
            }

            if (result.IsNotAllowed)
            {
                PageErrorMessage = _localizer["The user is not allowed to log in"];
                return Page();
            }

            // wrong username or password
            PageErrorMessage = _localizer["Your credential is invalid"];
            return Page();
        }

        public async Task<IActionResult> OnGetCancelAsync()
        {
            var context = await _interaction.GetAuthorizationContextAsync(ReturnUrl);
            if (context != null)
            {
                await _interaction.DenyAuthorizationAsync(context, AuthorizationError.AccessDenied);
                return Redirect(ReturnUrl);
            }

            return Redirect("~/");
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