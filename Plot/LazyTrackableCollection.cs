using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Plot.Metadata;
using Plot.Queries;

namespace Plot
{
    public class LazyTrackableCollection<T> : ITrackableCollection<T>
    {
        private readonly object _parent;

        private readonly Func<IQuery<T>> _factory;

        private readonly TrackableCollection<T> _underlyingData;

        private bool _populated;

        public LazyTrackableCollection(Func<IQuery<T>> factory, object parent, RelationshipMetadata relationship)
        {
            _factory = factory;
            _parent = parent;
            //_underlyingData = new TrackableCollection<T>(parent, relationship, new List<T>());
            Relationship = relationship;
        }

        public IEnumerable Flush()
        {
            return new List<T>();
        }
        
        public IEnumerator<T> GetEnumerator()
        {
            /*var parentState = EntityStateTracker.Get(_parent);

            if (parentState.Status == EntityStatus.New || _populated || parentState.Session.Item == null)
            {              

                foreach (var item in _underlyingData)
                {
                    yield return item;
                }
            }
            else
            {
                var query = _factory();

                _populated = true;

                foreach (var item in parentState.Session.Item.Query(query).Stream())
                {
                    var existing = _underlyingData.SingleOrDefault(x => x.Equals(item));

                    if (existing == null)
                    {
                        _underlyingData.Add(item);

                        existing = item;
                    }

                    yield return existing;
                }
            }*/
            return null;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool Contains(T item)
        {
            return _underlyingData.Contains(item);
        }

        public void Add(T item)
        {
            _underlyingData.Add(item);
        }

        public void Remove(T item)
        {
            _underlyingData.Remove(item);
        }

        public IEnumerable<T> Populate()
        {
            return _underlyingData.Populate();
        }

        public RelationshipMetadata Relationship { get; }
    }
}
