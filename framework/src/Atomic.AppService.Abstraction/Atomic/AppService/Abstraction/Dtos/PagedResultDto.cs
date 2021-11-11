using System;
using System.Collections.Generic;

namespace Atomic.AppService.Abstraction.Atomic.AppService.Abstraction.Dtos
{
    [Serializable]
    public class PagedResultDto<T> : ListResultDto<T>
    {
        public PagedResultDto()
        {
        }

        public PagedResultDto(long totalCount, IReadOnlyList<T> items)
            : base(items)
        {
            TotalCount = totalCount;
        }

        public long TotalCount { get; set; }
    }
}