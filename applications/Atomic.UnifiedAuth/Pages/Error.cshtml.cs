using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Atomic.UnifiedAuth.Pages
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class ErrorPageModel : PageModel
    {
        public string Path { get; set; }
        public string ErrorMessage { get; set; }

        public void OnGet()
        {
            var exceptionInfo = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            Path = exceptionInfo?.Path;
            ErrorMessage = exceptionInfo?.Error.Message;
        }
    }
}