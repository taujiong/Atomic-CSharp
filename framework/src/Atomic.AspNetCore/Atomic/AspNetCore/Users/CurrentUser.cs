using System;
using System.Linq;
using System.Security.Claims;
using Atomic.AspNetCore.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Atomic.AspNetCore.Users
{
    public class CurrentUser : ICurrentUser
    {
        private static readonly Claim[] EmptyClaimArray = Array.Empty<Claim>();
        private readonly ClaimMapOption _claimMap;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUser(
            IHttpContextAccessor httpContextAccessor,
            IOptions<ClaimMapOption> claimMapOptions
        )
        {
            _httpContextAccessor = httpContextAccessor;
            _claimMap = claimMapOptions.Value;
        }

        public bool IsAuthenticated => !string.IsNullOrEmpty(Id);
        public string? Id => FindClaim(_claimMap.UserId)?.Value;

        public string? UserName => FindClaim(_claimMap.UserName)?.Value;

        public string? PhoneNumber => FindClaim(_claimMap.PhoneNumber)?.Value;

        public bool PhoneNumberVerified => string.Equals(FindClaim(_claimMap.PhoneNumberVerified)?.Value, "true",
            StringComparison.InvariantCulture);

        public string? Email => FindClaim(_claimMap.Email)?.Value;

        public bool EmailVerified => string.Equals(FindClaim(_claimMap.EmailVerified)?.Value, "true",
            StringComparison.InvariantCulture);

        public string[] Roles => FindClaims(_claimMap.Role).Select(c => c.Value).Distinct().ToArray();

        public Claim? FindClaim(string claimType)
        {
            return _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == claimType);
        }

        public Claim[] FindClaims(string claimType)
        {
            return _httpContextAccessor.HttpContext?.User.Claims.Where(c => c.Type == claimType).ToArray() ??
                   EmptyClaimArray;
        }

        public Claim[] GetAllClaims()
        {
            return _httpContextAccessor.HttpContext?.User.Claims.ToArray() ?? EmptyClaimArray;
        }
    }
}