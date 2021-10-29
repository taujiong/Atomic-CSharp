using System.Threading.Tasks;
using Atomic.UnifiedAuth.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Atomic.UnifiedAuth.Pages.Account
{
    public class LogoutPageModel : AccountPageModel
    {
        private readonly SignInManager<AppUser> _signInManager;

        public LogoutPageModel(SignInManager<AppUser> signInManager)
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