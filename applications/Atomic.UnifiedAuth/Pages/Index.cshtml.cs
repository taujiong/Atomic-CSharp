using Atomic.AspNetCore.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Atomic.UnifiedAuth.Pages
{
    public class IndexPageModel : PageModel
    {
        private readonly ICurrentUser _currentUser;

        public IndexPageModel(ICurrentUser currentUser)
        {
            _currentUser = currentUser;
        }

        public IActionResult OnGet()
        {
            if (_currentUser.IsAuthenticated)
            {
                return RedirectToPage("./Profile");
            }

            return Page();
        }
    }
}