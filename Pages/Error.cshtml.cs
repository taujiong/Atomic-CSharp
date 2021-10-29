using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Atomic.UnifiedAuth.Pages
{
    public class ErrorPageModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int ErrorCode { get; set; } = 500;

        [BindProperty(SupportsGet = true)]
        public string ErrorMessage { get; set; }
    }
}