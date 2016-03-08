using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Plot.Metadata;

namespace Plot
{
    public class TrackableCollection<T> : IList<T>, ITrackableCollection<T>
    {
        private readonly List<T> _data;

        private readonly List<T> _removed;

        private readonly object _parent;

        private readonly RelationshipMetadata _relationship;
        
        public TrackableCollection(object parent, RelationshipMetadata relatioship, IEnumerable<T> source)
        {
            _data = new List<T>();
            _removed = new List<T>();
            _parent = parent;
            _relationship = relatioship;
            Add(source);
        }

        public IEnumerable Flush()
        {
            var items = _removed.ToArray();
            _removed.Clear();
            return items;
        }

        public int IndexOf(T item)
        {
            return _data.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            _data.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _removed.Add(_data[index]);

            _data.RemoveAt(index);
        }

        public T this[int index]
        {
            get { return _data[index]; }
            set { _data[index] = value; }
        }

        public void Add(T item)
        {
            AddInternal(item);
            var parentState = EntityStateTracker.Get(_parent);
            parentState.Dirty();
        }

        public void Clear()
        {
            _data.Clear();
        }

        public bool Contains(T item)
        {
            return _data.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _data.CopyTo(array, arrayIndex);
        }

        public int Count => _data.Count;

        public bool IsReadOnly => false;

        public bool Remove(T item)
        {
            _data.Remove(item);
            _removed.Add(item);
            EntityStateTracker.Get(_parent).Dirty();
            return true;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        public IEnumerable<T> Removed => _removed;

        public IEnumerable<T> Populate()
        {
            var parentState = EntityStateTracker.Get(_parent);
            var session = parentState.Session.Item;

            if (session == null)
            {
                throw new NullReferenceException("Session must be set before calling populate");
            }

            return session.Get<T>(_data.Select(x => EntityStateTracker.Get(x).GetIdentifier()).ToArray());
        }

        public RelationshipMetadata Relationship => _relationship;

        private void Add(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                AddInternal(item);
            }
        }

        private void AddInternal(T item)
        {
            if (item == null)
            {
                throw new InvalidOperationException("A NULL item cannot be added to a trackable collection");
            }
            _data.Add(item);
            var parentState = EntityStateTracker.Get(_parent);
            parentState.Session.Get(session =>
            {
                if (_relationship != null && _relationship.IsReverse)
                {
                    var itemState = EntityStateTracker.Get(item);
                    parentState.Dependencies.Register(itemState.Dependencies);
                }
            });
        }
    }
}
