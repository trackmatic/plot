using System.Collections;
using System.Collections.Generic;
using Plot.Metadata;

namespace Plot
{
    public interface ITrackableCollection : IEnumerable
    {
        IEnumerable Flush();
    }

    public interface ITrackableCollection<T> : IEnumerable<T>, ITrackableCollection
    {

        IEnumerable<T> Populate();

        RelationshipMetadata Relationship { get; }
    }
}
