using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Atomic.UnifiedAuth.Users
{
    public class UserClaimsPrincipalFactory : UserClaimsPrincipalFactory<AppUser, IdentityRole>
    {
        public UserClaimsPrincipalFactory(
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<IdentityOptions> optionsAccessor
        ) : base(userManager, roleManager, optionsAccessor)
        {
        }

        public override async Task<ClaimsPrincipal> CreateAsync(AppUser user)
        {
            var principal = await base.CreateAsync(user);
            var identity = (ClaimsIdentity)principal.Identity;

            if (identity == null) return principal;

            // there are two email claims
            // one comes from AspNetCore with type ClaimTypes.Email
            // another from IdentityServer4 with type JwtClaimTypes.Email
            // we want to keep the latter, so remove the former
            var emailClaim = identity.FindFirst(c => c.Type == ClaimTypes.Email);
            identity.RemoveClaim(emailClaim);
            identity.AddClaim(new Claim(JwtClaimTypes.Picture, user.AvatarUrl));

            return principal;
        }
    }
}