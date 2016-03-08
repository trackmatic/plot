using System;
using System.Collections.Generic;

namespace Plot
{
    public class TrackableRelationship<T> : ITrackableRelationship<T>, IEquatable<T>
    {
        private readonly List<T> _removed;

        private T _data;

        private readonly object _parent;

        private readonly bool _reverse;

        public TrackableRelationship(object parent, bool reverse)
        {       
            _removed = new List<T>();

            _parent = parent;

            _reverse = reverse;
        }

        public IEnumerable<T> Flush()
        {
            var items = _removed.ToArray();
            _removed.Clear();
            return items;
        }

        public string Id
        {
            get
            {
                if (!HasValue())
                {
                    return null;
                }

                return EntityStateTracker.Get(_data).GetIdentifier();
            }
        }

        public Type Type => _data.GetType();

        public T Get()
        {
            return _data;
        }
        
        public void Set(T data)
        {
            if (_data != null)
            {
                _removed.Add(_data);
            }
            var parentState = EntityStateTracker.Get(_parent);
            parentState.Session.Get(session =>
            {
                if (data == null)
                {
                    return;
                }

                if (_reverse)
                {
                    var childState = EntityStateTracker.Get(data);
                    parentState.Dependencies.Register(childState.Dependencies);
                }
            });
            _data = data;
            parentState.Dirty();
        }

        public IEnumerable<T> Removed
        {
            get { return _removed; }
        }

        public bool HasValue()
        {
            return _data != null;
        }

        public bool Equals(T obj)
        {
            if (!HasValue())
            {
                return false;
            }

            return _data.Equals(obj);
        }

        public void Get(Action<T> item, bool returnNull = false)
        {
            if (!returnNull && !HasValue())
            {
                return;
            }

            item(_data);
        }

        public void Populate(Action<T> item, bool returnNull = false)
        {
            if (!returnNull && !HasValue())
            {
                return;
            }

            if (!HasValue())
            {
                item(default(T));

                return;
            }

            var state = EntityStateTracker.Get(_data);

            if (state.IsPopulated)
            {
                item(_data);
            }

            state.Session.Get(x => item( x.Get<T>(state.GetIdentifier())));
        }
    }
}
