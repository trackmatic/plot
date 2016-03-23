using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Plot
{
    public class EntityStateCache : IEntityStateCache
    {
        private readonly IDictionary<object, EntityState> _state;

        public EntityStateCache()
        {

            _state = new ConcurrentDictionary<object, EntityState>();
        }

        public EntityState Create(object proxy)
        {
            var item = ProxyUtils.GetTargetEntity(proxy);
            var state = new EntityState(ProxyUtils.GetEntityId(proxy));
            _state.Add(item, state);
            return state;
        }

        public EntityState Get(object proxy)
        {
            var item = ProxyUtils.GetTargetEntity(proxy);
            return _state[item];
        }

        public bool Contains(object proxy)
        {
            var item = ProxyUtils.GetTargetEntity(proxy);
            if (item == null)
            {
                return false;
            }
            return _state.ContainsKey(item);
        }

        public void Remove(object proxy)
        {
            var item = ProxyUtils.GetTargetEntity(proxy);
            _state.Remove(item);
        }

        public void Dispose()
        {
            _state.Clear();
        }

        public IEnumerable<EntityState> State => _state.Values;
    }
}