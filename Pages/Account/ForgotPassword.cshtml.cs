using System.Threading.Tasks;
using Atomic.UnifiedAuth.Localization;
using Atomic.UnifiedAuth.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Atomic.UnifiedAuth.Pages.Account
{
    public class ForgotPassword : PageModel
    {
        private readonly IStringLocalizer<AccountResource> _localizer;
        private readonly ILogger<ForgotPassword> _logger;
        private readonly UserManager<AppUser> _userManager;

        public ForgotPassword(
            UserManager<AppUser> userManager,
            IStringLocalizer<AccountResource> localizer,
            ILogger<ForgotPassword> logger
        )
        {
            _userManager = userManager;
            _localizer = localizer;
            _logger = logger;
        }

        [BindProperty]
        public ForgotPasswordModel Input { get; set; }

        public bool LinkSent { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var user = await _userManager.FindByEmailAsync(Input.EmailAddress);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, _localizer["Can not find a user with given email address"]);
                return Page();
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            // code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "./ResetPassword",
                null,
                new { code, userId = user.Id }
            );

            // TODO: implement with IEmailSender
            _logger.LogInformation(callbackUrl);
            LinkSent = true;

            return Page();
        }
    }
}