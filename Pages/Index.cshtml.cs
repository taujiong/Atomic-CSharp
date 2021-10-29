using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Atomic.UnifiedAuth.Pages
{
    public class IndexPageModel : PageModel
    {
        public IActionResult OnGet()
        {
            if (User.Identity is { IsAuthenticated: true })
            {
                return RedirectToPage("./Profile");
            }

            return Page();
        }
    }
}