﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Plot
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly List<object> _items;

        private readonly IEntityStateCache _entityStateCache;

        public UnitOfWork(IEntityStateCache entityStateCache)
        {
            _items = new List<object>();
            _entityStateCache = entityStateCache;
        }
        
        public void Remove(object item)
        {
            if (_items.Contains(item))
            {
                return;
            }
            _entityStateCache.Remove(item);
            _items.Remove(item);
        }

        public void Merge(object item)
        {
            var state = _entityStateCache.Get(item);

            if (state.IsReadonly)
            {
                return;
            }

            foreach (var other in _items.Where(x => x.Equals(item)).ToList())
            {
                _items.Remove(other);
            }

            _items.Add(item);
        }

        public void Register(object item)
        {
            var state = _entityStateCache.Get(item);
            if (state.IsReadonly)
            {
                return;
            }
            if (_items.Contains(item))
            {
                return;
            }
            _items.Add(item);
            OnRegistered(item);
        }

        public IEnumerable<object> Items => _items;

        public IEnumerable<object> Flush()
        {
            OnFlushing();

            foreach (var item in Items.OrderBy(x => _entityStateCache.Get(x).Dependencies.Sequence))
            {
                yield return item;
            }

            OnFlushed();
        }

        public bool Contains(object item)
        {
            return _items.Contains(item);
        }

        public T Get<T>(string id)
        {
            return (T)_items.SingleOrDefault(x => x is T && _entityStateCache.Get(x).GetIdentifier().Equals(id));
        }

        protected virtual void OnRegistered(object item)
        {
            
        }

        protected virtual void OnFlushed()
        {
            
        }

        protected virtual void OnFlushing()
        {
            
        }

        public void Dispose()
        {
            foreach (var item in _items.ToList())
            {
                Remove(item);
            }

            _items.Clear();
        }

        protected object Get(string id, Type type)
        {
            return _items.SingleOrDefault(x => x.GetType() == type && _entityStateCache.Get(x).GetIdentifier().Equals(id));
        }
    }
}
