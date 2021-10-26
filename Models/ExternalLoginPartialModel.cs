using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;

namespace Atomic.UnifiedAuth.Models
{
    public class ExternalLoginPartialModel
    {
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string Action { get; set; }

        public string ReturnUrl { get; set; }
    }
}