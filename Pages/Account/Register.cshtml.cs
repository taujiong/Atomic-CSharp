using System.Linq;
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
    public class Register : PageModel
    {
        private readonly IStringLocalizer<AccountResource> _localizer;
        private readonly ILogger<Register> _logger;
        private readonly UserManager<AppUser> _userManager;

        public Register(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ILogger<Register> logger,
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

        public string ReturnUrl { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool IsExternal { get; set; }

        public string ErrorMessage { get; set; }

        public IActionResult OnGetAsync(string username, string emailAddress, string returnUrl = null)
        {
            Input = new RegisterInputModel
            {
                Username = username,
                EmailAddress = emailAddress,
            };

            ReturnUrl = returnUrl ?? Url.Content("~/");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
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
                        ErrorMessage = _localizer[message];

                        return Page();
                    }

                    result = await _userManager.AddLoginAsync(user, loginInfo);
                    if (!result.Succeeded)
                    {
                        ErrorMessage = result.Errors.First().Description;
                        return Page();
                    }
                }

                await SignInManager.SignInAsync(user, false);
                return Redirect(returnUrl);
            }

            ErrorMessage = result.Errors.First().Description;
            return Page();
        }
    }
}