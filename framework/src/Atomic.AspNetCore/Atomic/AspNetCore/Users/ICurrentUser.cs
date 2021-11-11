using System.Security.Claims;
using JetBrains.Annotations;

namespace Atomic.AspNetCore.Users
{
    public interface ICurrentUser
    {
        bool IsAuthenticated { get; }

        [CanBeNull]
        string Id { get; }

        [CanBeNull]
        string UserName { get; }

        [CanBeNull]
        string AvatarUrl { get; }

        [CanBeNull]
        string PhoneNumber { get; }

        bool PhoneNumberVerified { get; }

        [CanBeNull]
        string Email { get; }

        bool EmailVerified { get; }

        string[] Roles { get; }

        [CanBeNull]
        Claim FindClaim(string claimType);

        Claim[] FindClaims(string claimType);

        Claim[] GetAllClaims();
    }
}