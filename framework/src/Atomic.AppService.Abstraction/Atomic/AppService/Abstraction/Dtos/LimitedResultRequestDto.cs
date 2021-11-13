using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Atomic.AppService.Abstraction.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace Atomic.AppService.Abstraction.Dtos
{
    [Serializable]
    public class LimitedResultRequestDto : IValidatableObject
    {
        /// <summary>
        /// Default value: 10.
        /// </summary>
        public static int DefaultMaxResultCount { get; set; } = 10;

        /// <summary>
        /// Maximum possible value of the <see cref="MaxResultCount"/>.
        /// Default value: 1,000.
        /// </summary>
        public static int MaxMaxResultCount { get; set; } = 1000;

        /// <summary>
        /// Maximum result count should be returned.
        /// This is generally used to limit result count on paging.
        /// </summary>
        [Range(1, int.MaxValue)]
        public virtual int MaxResultCount { get; set; } = DefaultMaxResultCount;

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (MaxResultCount > MaxMaxResultCount)
            {
                var localizer = validationContext.GetRequiredService<IStringLocalizer<AtomicAppServiceResource>>();

                yield return new ValidationResult(
                    localizer[
                        "{0} can not be more than {1}! Increase {2}.{3} on the server side to allow more results.",
                        nameof(MaxResultCount),
                        MaxMaxResultCount,
                        typeof(LimitedResultRequestDto).FullName!,
                        nameof(MaxMaxResultCount)
                    ],
                    new[] { nameof(MaxResultCount) });
            }
        }
    }
}