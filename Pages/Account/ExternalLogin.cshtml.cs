﻿using System.Security.Claims;
using System.Threading.Tasks;
using Atomic.UnifiedAuth.Localization;
using Atomic.UnifiedAuth.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Atomic.UnifiedAuth.Pages.Account
{
    public class ExternalLogin : AccountPageModel
    {
        private readonly IStringLocalizer<AccountResource> _localizer;
        private readonly ILogger<ExternalLogin> _logger;
        private readonly SignInManager<AppUser> _signInManager;

        public ExternalLogin(
            SignInManager<AppUser> signInManager,
            ILogger<ExternalLogin> logger,
            IStringLocalizer<AccountResource> localizer
        )
        {
            _signInManager = signInManager;
            _logger = logger;
            _localizer = localizer;
        }

        public IActionResult OnPost(string scheme)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Page("./ExternalLogin", "Callback", new { ReturnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(scheme, redirectUrl);
            return new ChallengeResult(scheme, properties);
        }

        public async Task<IActionResult> OnGetCallbackAsync(string remoteError = null)
        {
            if (remoteError != null)
            {
                _logger.LogWarning("External login failed: {error}", remoteError);
                ServerErrorMessage = remoteError;
                return LocalRedirect("/Error");
            }

            var loginInfo = await _signInManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                const string message = "Error loading external login information";
                _logger.LogWarning(message);
                ServerErrorMessage = _localizer[message];
                return LocalRedirect("/Error");
            }

            var username = loginInfo.Principal.FindFirstValue(ClaimTypes.Name);
            var emailAddress = loginInfo.Principal.FindFirstValue(ClaimTypes.Email);

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(loginInfo.LoginProvider, loginInfo.ProviderKey,
                false, true);

            if (result.Succeeded)
            {
                return RedirectSafely();
            }

            if (result.IsLockedOut)
            {
                ServerErrorMessage = _localizer["The user is locked out, re-try in 5 minutes"];
                return LocalRedirect("/Error");
            }

            if (result.IsNotAllowed)
            {
                ServerErrorMessage = _localizer["The user is not allowed to log in"];
                return LocalRedirect("/Error");
            }

            // If the user does not have an account, then redirect to register page.
            return RedirectToPage("./Register", new
            {
                IsExternal = true,
                username,
                emailAddress,
                ReturnUrl
            });
        }
    }
}