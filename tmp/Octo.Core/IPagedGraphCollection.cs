using System;
using System.Collections.Generic;

namespace Octo.Core
{
    public interface IPagedGraphCollection<T>
    {
        IEnumerable<T> Data { get; set; }

        int Total { get; set; }

        int Page { get; set; }

        bool IsEmpty { get; }

        IPagedGraphCollection<T> Next();

        IEnumerable<T> Stream();

        K Map<K>(Func<IPagedGraphCollection<T>, K> map);
    }
}
