using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Atomic.UnifiedAuth.Models
{
    public class ResetPasswordModel
    {
        [HiddenInput]
        public string UserId { get; set; }

        [HiddenInput]
        public string Code { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}