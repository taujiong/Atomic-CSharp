using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Atomic.UnifiedAuth.Localization;
using Atomic.UnifiedAuth.Models;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Atomic.UnifiedAuth.Pages
{
    public class ConsentPageModel : PageModel
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IStringLocalizer<AccountResource> _localizer;
        private readonly ILogger<ConsentPageModel> _logger;

        public ConsentPageModel(
            IIdentityServerInteractionService interaction,
            IStringLocalizer<AccountResource> localizer,
            ILogger<ConsentPageModel> logger
        )
        {
            _interaction = interaction;
            _localizer = localizer;
            _logger = logger;
        }

        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }

        [BindProperty]
        public ConsentInputModel Input { get; set; }

        public string UserName { get; set; }

        public ClientModel ConsentClient { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var context = await GetAuthorizationContextAsync(ReturnUrl);

            UserName = User.Identity?.Name;
            ConsentClient = new ClientModel(context.Client);
            Input = CreateConsentInputModel(context);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string userDecision)
        {
            var context = await GetAuthorizationContextAsync(ReturnUrl);

            var grantedConsent = "no".Equals(userDecision)
                ? new ConsentResponse { Error = AuthorizationError.AccessDenied }
                : new ConsentResponse
                {
                    RememberConsent = Input.RememberConsent,
                    ScopesValuesConsented = Input.GetAllowedScopeNames(),
                };

            await _interaction.GrantConsentAsync(context, grantedConsent);

            return Redirect(ReturnUrl);
        }

        private async Task<AuthorizationRequest> GetAuthorizationContextAsync(string returnUrl)
        {
            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
            if (context == null)
            {
                _logger.LogWarning("No consent request matching request: {0}", ReturnUrl);
                throw new Exception(_localizer["Invalid login request from third-party client."]);
            }

            return context;
        }

        private ConsentInputModel CreateConsentInputModel(AuthorizationRequest context)
        {
            var identityScopes = context.ValidatedResources.Resources.IdentityResources
                .Select(x => CreateScopeViewModel(x, true))
                .ToList();

            var apiScopes = new List<ScopeViewModel>();
            foreach (var parsedScope in context.ValidatedResources.ParsedScopes)
            {
                var apiScope = context.ValidatedResources.Resources.FindApiScope(parsedScope.ParsedName);
                if (apiScope == null) continue;
                var scopeVm = CreateScopeViewModel(parsedScope, apiScope, true);
                apiScopes.Add(scopeVm);
            }

            if (context.ValidatedResources.Resources.OfflineAccess)
                apiScopes.Add(GetOfflineAccessScope(true));

            return new ConsentInputModel
            {
                IdentityScopes = identityScopes,
                ApiScopes = apiScopes,
                RememberConsent = true,
            };
        }

        private static ScopeViewModel CreateScopeViewModel(IdentityResource identity, bool check)
        {
            return new ScopeViewModel
            {
                Name = identity.Name,
                DisplayName = identity.DisplayName,
                Description = identity.Description,
                Emphasize = identity.Emphasize,
                Required = identity.Required,
                Checked = check || identity.Required,
            };
        }

        private static ScopeViewModel CreateScopeViewModel(
            ParsedScopeValue parsedScopeValue,
            ApiScope apiScope,
            bool check
        )
        {
            var displayName = apiScope.DisplayName ?? apiScope.Name;
            if (!string.IsNullOrWhiteSpace(parsedScopeValue.ParsedParameter))
                displayName += ":" + parsedScopeValue.ParsedParameter;

            return new ScopeViewModel
            {
                Name = parsedScopeValue.RawValue,
                DisplayName = displayName,
                Description = apiScope.Description,
                Emphasize = apiScope.Emphasize,
                Required = apiScope.Required,
                Checked = check || apiScope.Required,
            };
        }

        private ScopeViewModel GetOfflineAccessScope(bool check)
        {
            return new ScopeViewModel
            {
                Name = IdentityServerConstants.StandardScopes.OfflineAccess,
                DisplayName = "Offline Access",
                Description = _localizer["Access to your applications and resources, even when you are offline"],
                Emphasize = true,
                Checked = check,
            };
        }
    }
}