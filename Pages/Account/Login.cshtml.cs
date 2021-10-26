using System.Threading.Tasks;
using Atomic.UnifiedAuth.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Atomic.UnifiedAuth.Pages.Account
{
    public class Login : PageModel
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;

        public Login(
            SignInManager<AppUser> signInManager,
            UserManager<AppUser> userManager
        )
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.FindByNameAsync("bob");
            var result = await _signInManager.PasswordSignInAsync(user, "Pass123$", true, true);
            if (result.Succeeded) return Redirect("~/");

            return Page();
        }
    }
}