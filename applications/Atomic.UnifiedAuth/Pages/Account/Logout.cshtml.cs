using System.Threading.Tasks;
using Atomic.UnifiedAuth.Models;
using IdentityServer4.Extensions;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Atomic.UnifiedAuth.Pages.Account
{
    public class LogoutPageModel : AccountPageModel
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly SignInManager<AppUser> _signInManager;

        public LogoutPageModel(
            SignInManager<AppUser> signInManager,
            IIdentityServerInteractionService interaction
        )
        {
            _signInManager = signInManager;
            _interaction = interaction;
        }

        public string PostLogoutRedirectUri { get; set; }

        public string ClientName { get; set; }

        public string SignOutIframeUrl { get; set; }

        public async Task<IActionResult> OnGetAsync(string logoutId)
        {
            if (User.IsAuthenticated())
            {
                await _signInManager.SignOutAsync();
            }

            if (string.IsNullOrEmpty(logoutId)) return RedirectSafely();

            var logoutContext = await _interaction.GetLogoutContextAsync(logoutId);
            PostLogoutRedirectUri = logoutContext.PostLogoutRedirectUri;
            ClientName = logoutContext.ClientName ?? logoutContext.ClientId;
            SignOutIframeUrl = logoutContext.SignOutIFrameUrl;

            return Page();
        }
    }
}