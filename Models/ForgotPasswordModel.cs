using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Atomic.UnifiedAuth.Models
{
    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        [BindProperty]
        [Display(Name = "Email address")]
        public string EmailAddress { get; set; }
    }
}