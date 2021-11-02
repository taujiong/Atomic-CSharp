using Microsoft.AspNetCore.Identity;

namespace Atomic.UnifiedAuth.Models
{
    public class AppUser : IdentityUser
    {
        public AppUser()
        {
        }

        public AppUser(string userName) : base(userName)
        {
        }
    }
}