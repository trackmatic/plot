using System;
using System.Collections.Generic;
using System.Linq;
using Plot.Proxies;
using Plot.Queries;

namespace Plot
{
    public abstract class AbstractRepository<T> : IRepository<T> where T : class
    {
        private readonly IMapper<T> _mapper;

        private readonly IGraphSession _session;

        private const int Max = 100;

        private int _counter;

        private readonly IProxyFactory _proxyFactory;

        private readonly IEntityStateCache _entityStateCache;

        protected AbstractRepository(IMapper<T> mapper, IGraphSession session, IProxyFactory proxyFactory)
        {
            _mapper = mapper;
            _session = session;
            _proxyFactory = proxyFactory;
            _entityStateCache = session.State;
        }

        public object Create(object item)
        {
            var proxy = _proxyFactory.Create((T)item, _session, EntityStatus.New);
            _session.Register(proxy);
            return proxy;
        }

        public void Delete(object item)
        {
            _entityStateCache.Get(item).Delete();
        }

        public void Store(T item)
        {
            var proxy = _proxyFactory.Create(item, _session);
            _entityStateCache.Get(proxy).New();
        }

        public void Delete(T item)
        {
            _entityStateCache.Get(item).Delete();
        }
        
        public IEnumerable<T> Get(params string[] id)
        {
            var items = LoadFromUow(id);
            if (IsPopulated(items))
            {
                return items.Select(x => x.Value);
            }
            if (_counter > Max)
            {
                throw new InvalidOperationException($"Exceeded maximum number of reads per session ({Max})");
            }
            _counter++;
            var identifiers = GetUnpopulatedItems(items);
            foreach (var item in _mapper.Get(identifiers))
            {
                var proxy = _proxyFactory.Create(item, _session);
                var state = _entityStateCache.Get(item);
                items[state.GetIdentifier()] = proxy;
                state.Populate();
            }
            return items.Select(x => x.Value);
        }

        public IMapper Mapper => _mapper;

        public void Dispose()
        {
            _counter = 0;
        }

        private IDictionary<string, T> LoadFromUow(IEnumerable<string> id)
        {
            var items = id.Select(x => new
            {
                Id = x,
                Item = _session.Uow.Get<T>(x)
            }).ToDictionary(pair => pair.Id, pair => pair.Item);
            return items;
        }

        private bool IsPopulated(IEnumerable<KeyValuePair<string, T>> items)
        {
            return items.All(x => x.Value != null && _entityStateCache.Get(x.Value).IsPopulated);
        }

        private string[] GetUnpopulatedItems(IDictionary<string, T> items)
        {
            return items.Where(x => x.Value == null || !_entityStateCache.Get(x.Value).IsPopulated).Select(x => x.Key).ToArray();
        }
    }
}
