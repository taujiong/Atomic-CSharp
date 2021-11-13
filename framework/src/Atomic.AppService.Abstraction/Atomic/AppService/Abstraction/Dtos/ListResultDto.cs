using System;
using System.Collections.Generic;

namespace Atomic.AppService.Abstraction.Dtos
{
    [Serializable]
    public class ListResultDto<T>
    {
        private IReadOnlyList<T> _items;

        public ListResultDto()
        {
        }

        public ListResultDto(IReadOnlyList<T> items)
        {
            Items = items;
        }

        public IReadOnlyList<T> Items
        {
            get { return _items ??= new List<T>(); }
            set => _items = value;
        }
    }
}