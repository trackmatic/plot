using System.Collections.Generic;
using Octo.Core.Metadata;

namespace Octo.Core
{
    public interface ITrackableCollection<T> : IEnumerable<T>
    {
        IEnumerable<T> Flush();

        IEnumerable<T> Populate();

        Relationship Relationship { get; }
    }
}
