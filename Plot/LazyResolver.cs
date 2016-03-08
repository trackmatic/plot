using System;
using System.Collections.Generic;

namespace Plot
{
    public class LazyResolver<T> where T : class
    {
        private readonly List<Action<T>> _callbacks;

        private T _item;

        public LazyResolver()
        {
            _callbacks = new List<Action<T>>();
        }

        public void Get(Action<T> callback)
        {
            if (_item == null)
            {
                _callbacks.Add(callback);
            }
            else
            {
                callback(_item);
            }
        }

        public void Ready(T item)
        {
            if (_item != null)
            {
                return;
            }

            _item = item;

            foreach (var callback in _callbacks)
            {
                callback(item);
            }

            _callbacks.Clear();
        }

        public T Item => _item;
    }
}
