using System.Collections.Generic;
using Plot.Proxies;
using Plot.Queries;

namespace Plot
{
    public class GraphSessionFactory : IGraphSessionFactory
    {
        private readonly IQueryExecutorFactory _queryExecutorFactory;

        private readonly IRepositoryFactory _repositoryFactory;

        private readonly List<IListener> _listeners;

        private readonly IEntityStateCacheFactory _entityStateFactory;

        private readonly IProxyFactory _proxyFactory;

        public GraphSessionFactory(IQueryExecutorFactory queryExecutorFactory, IRepositoryFactory repositoryFactory, IEntityStateCacheFactory entityStateFactory, IProxyFactory proxyFactory)
        {
            _queryExecutorFactory = queryExecutorFactory;
            _repositoryFactory = repositoryFactory;
            _listeners = new List<IListener>();
            _entityStateFactory = entityStateFactory;
            _proxyFactory = proxyFactory;
        }

        public IGraphSession OpenSession()
        {
            var entityStateCache = _entityStateFactory.Create();
            var uow = new UnitOfWork(entityStateCache);
            var session = new GraphSession(uow, _listeners, _queryExecutorFactory, _repositoryFactory, entityStateCache, _proxyFactory);
            return session;
        }
        
        public void Register(IListener listener)
        {
            if (_listeners.Contains(listener))
            {
                return;
            }

            _listeners.Add(listener);
        }
    }
}
