using System;

namespace Atomic.AppService.Abstraction.Dtos
{
    [Serializable]
    public class PagedAndSortedResultRequestDto : PagedResultRequestDto
    {
        public virtual string Sorting { get; set; }
    }
}