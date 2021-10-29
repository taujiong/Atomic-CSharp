using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Atomic.UnifiedAuth.Pages.Account
{
    public class AccountPageModel : PageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }

        public string PageErrorMessage { get; protected set; }

        protected IActionResult RedirectSafely()
        {
            return Redirect(ReturnUrl ?? "~/");
        }
    }
}