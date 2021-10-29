using System.Threading.Tasks;
using Atomic.UnifiedAuth.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Atomic.UnifiedAuth.Pages.Account
{
    public class Logout : AccountPageModel
    {
        private readonly SignInManager<AppUser> _signInManager;

        public Logout(SignInManager<AppUser> signInManager, ILogger<Logout> logger)
        {
            _signInManager = signInManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            await _signInManager.SignOutAsync();

            return Redirect("~/");
        }
    }
}