using Microsoft.AspNetCore.Identity;

namespace Atomic.UnifiedAuth.Users
{
    public class AppUser : IdentityUser
    {
        public AppUser()
        {
        }

        public AppUser(string userName) : base(userName)
        {
        }

        [PersonalData]
        public string AvatarUrl { get; set; }
    }
}