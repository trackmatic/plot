using System;
using System.Collections.Generic;
using System.Linq;
using Octo.Core.Proxies;
using Octo.Core.Queries;

namespace Octo.Core
{
    public abstract class AbstractRepository<T> : IRepository<T> where T : class
    {
        private readonly IMapper<T> _mapper;

        private readonly IGraphSession _session;

        private const int Max = 100;

        private int _counter;

        private readonly IProxyFactory _proxyFactory;

        protected AbstractRepository(IMapper<T> mapper, IGraphSession session, IProxyFactory proxyFactory)
        {
            _mapper = mapper;
            _session = session;
            _proxyFactory = proxyFactory;
        }

        public void Store(object item)
        {
            var state = EntityStateTracker.Get(item);
            _session.Register(item, state);
            state.New();
        }

        public void Delete(object item)
        {
            EntityStateTracker.Get(item).Delete();
        }

        public void Store(T item)
        {
            var proxy = _proxyFactory.Create(item, _session);
            EntityStateTracker.Get(proxy).New();
        }

        public void Delete(T item)
        {
            EntityStateTracker.Get(item).Delete();
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
                var state = EntityStateTracker.Get(item);
                items[state.GetIdentifier()] = proxy;
                state.Populate();
            }
            return items.Select(x => x.Value);
        }

        public IPagedGraphCollection<T> Query(IQueryExecutor<T> queryExecutor, IQuery<T> query)
        {
            var item = queryExecutor.Execute(query);
            return item;
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
            return items.All(x => x.Value != null && EntityStateTracker.Get(x.Value).IsPopulated);
        }

        private string[] GetUnpopulatedItems(IDictionary<string, T> items)
        {
            return items.Where(x => x.Value == null || !EntityStateTracker.Get(x.Value).IsPopulated).Select(x => x.Key).ToArray();
        }
    }
}
