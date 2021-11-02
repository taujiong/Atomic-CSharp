using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;

namespace Atomic.UnifiedAuth
{
    public static class IdentityServerConfig
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new("scope1"),
                new("scope2"),
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new()
                {
                    ClientId = "mvc",
                    ClientName = "MVC web",
                    LogoUri = "favicon/android-chrome-512x512.png",

                    AllowedGrantTypes = GrantTypes.Code,
                    ClientSecrets = { new Secret("mvc".Sha256()) },

                    RedirectUris = { "https://localhost:5003/signin-oidc" },
                    FrontChannelLogoutUri = "https://localhost:5003/signout-oidc",
                    PostLogoutRedirectUris = { "https://localhost:5003/signout-callback-oidc" },

                    AllowOfflineAccess = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    AccessTokenLifetime = 5,
                    RequireConsent = true,

                    AllowedScopes =
                    {
                        "scope1",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                    },
                },
            };
    }
}