using System.Threading.Tasks;
using Atomic.UnifiedAuth.Localization;
using Atomic.UnifiedAuth.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Atomic.UnifiedAuth.Pages.Account
{
    public class ForgotPasswordPageModel : AccountPageModel
    {
        private readonly IStringLocalizer<AccountResource> _localizer;
        private readonly ILogger<ForgotPasswordPageModel> _logger;
        private readonly UserManager<AppUser> _userManager;

        public ForgotPasswordPageModel(
            UserManager<AppUser> userManager,
            IStringLocalizer<AccountResource> localizer,
            ILogger<ForgotPasswordPageModel> logger
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
                PageErrorMessage = _localizer["Can not find a user with given email address"];
                return Page();
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = Url.Page(
                "./ResetPassword",
                new { code, userId = user.Id }
            );

            // TODO: implement with IEmailSender
            _logger.LogInformation(callbackUrl);
            LinkSent = true;

            return Page();
        }
    }
}