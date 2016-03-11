using System.Collections;
using System.Collections.Generic;
using Plot.Metadata;

namespace Plot.Proxies
{
    public interface ITrackableCollection : IEnumerable, ITrackable
    {
    }

    public interface ITrackableCollection<T> : IEnumerable<T>, ITrackableCollection
    {

        IEnumerable<T> Populate();

        RelationshipMetadata Relationship { get; }
    }
}
