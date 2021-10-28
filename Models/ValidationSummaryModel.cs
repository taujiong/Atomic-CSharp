using Microsoft.AspNetCore.Mvc.Rendering;

namespace Atomic.UnifiedAuth.Models
{
    public class ValidationSummaryModel
    {
        public bool IsValid { get; set; }

        public ValidationSummary SummaryType { get; set; }
    }
}