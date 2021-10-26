using System.ComponentModel.DataAnnotations;

namespace Atomic.UnifiedAuth.Models
{
    public class LoginInputModel
    {
        [Required]
        [Display(Name = "Username or email address")]
        public string UsernameOrEmailAddress { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}