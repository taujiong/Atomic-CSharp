using System.ComponentModel.DataAnnotations;

namespace Atomic.UnifiedAuth.Models
{
    public class LoginInputModel
    {
        [Required]
        public string UsernameOrEmailAddress { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}