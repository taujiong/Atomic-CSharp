using System.ComponentModel.DataAnnotations;

namespace Atomic.UnifiedAuth.Models
{
    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email address")]
        public string EmailAddress { get; set; }
    }
}