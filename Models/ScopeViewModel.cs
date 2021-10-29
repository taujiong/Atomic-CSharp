using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Atomic.UnifiedAuth.Models
{
    public class ScopeViewModel
    {
        [Required]
        [HiddenInput]
        public string Name { get; set; }

        public bool Checked { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        public bool Emphasize { get; set; }

        public bool Required { get; set; }
    }
}