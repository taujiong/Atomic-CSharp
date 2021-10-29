using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Atomic.UnifiedAuth.Pages.Account
{
    public class AccountPageModel : PageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }

        [TempData]
        public string ServerErrorMessage { get; set; }

        public string PageErrorMessage { get; protected set; }

        protected IActionResult RedirectSafely()
        {
            return Redirect(ReturnUrl ?? "~/");
        }

        protected IActionResult RedirectToError(string errorMessage)
        {
            ServerErrorMessage = errorMessage;
            return LocalRedirect("/Error");
        }
    }
}