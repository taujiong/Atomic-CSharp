using System.Linq;
using System.Threading.Tasks;
using Atomic.UnifiedAuth.Localization;
using Atomic.UnifiedAuth.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Atomic.UnifiedAuth.Pages.Account
{
    public class RegisterPageModel : AccountPageModel
    {
        private readonly IStringLocalizer<AccountResource> _localizer;
        private readonly ILogger<RegisterPageModel> _logger;
        private readonly UserManager<AppUser> _userManager;

        public RegisterPageModel(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ILogger<RegisterPageModel> logger,
            IStringLocalizer<AccountResource> localizer
        )
        {
            _userManager = userManager;
            SignInManager = signInManager;
            _logger = logger;
            _localizer = localizer;
        }

        public SignInManager<AppUser> SignInManager { get; }

        [BindProperty]
        public RegisterInputModel Input { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool IsExternal { get; set; }

        public IActionResult OnGetAsync(string username, string emailAddress)
        {
            Input = new RegisterInputModel
            {
                Username = username,
                EmailAddress = emailAddress,
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var user = new AppUser { UserName = Input.Username, Email = Input.EmailAddress };
            var result = await _userManager.CreateAsync(user, Input.Password);
            if (result.Succeeded)
            {
                if (IsExternal)
                {
                    var loginInfo = await SignInManager.GetExternalLoginInfoAsync();
                    if (loginInfo == null)
                    {
                        const string message = "Error loading external login information";
                        _logger.LogWarning(message);
                        PageErrorMessage = _localizer[message];

                        return Page();
                    }

                    result = await _userManager.AddLoginAsync(user, loginInfo);
                    if (!result.Succeeded)
                    {
                        PageErrorMessage = result.Errors.First().Description;
                        return Page();
                    }
                }

                await SignInManager.SignInAsync(user, false);
                return RedirectSafely();
            }

            PageErrorMessage = result.Errors.First().Description;
            return Page();
        }
    }
}