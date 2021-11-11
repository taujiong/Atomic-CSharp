using System;
using System.ComponentModel.DataAnnotations;

namespace Atomic.AppService.Abstraction.Atomic.AppService.Abstraction.Dtos
{
    [Serializable]
    public class PagedResultRequestDto : LimitedResultRequestDto
    {
        [Range(0, int.MaxValue)]
        public virtual int SkipCount { get; set; }
    }
}