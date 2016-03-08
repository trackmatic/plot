using System.Collections.Generic;
using Plot.Queries;

namespace Plot
{
    public class GraphSessionFactory : IGraphSessionFactory
    {
        private readonly IQueryExecutorFactory _queryExecutorFactory;

        private readonly IRepositoryFactory _repositoryFactory;

        private readonly List<IListener> _listeners;

        private readonly IEntityStateCacheFactory _entityStateFactory;

        public GraphSessionFactory(IQueryExecutorFactory queryExecutorFactory, IRepositoryFactory repositoryFactory, IEntityStateCacheFactory entityStateFactory)
        {
            _queryExecutorFactory = queryExecutorFactory;
            _repositoryFactory = repositoryFactory;
            _listeners = new List<IListener>();
            _entityStateFactory = entityStateFactory;
        }

        public IGraphSession OpenSession()
        {
            var entityStateCache = _entityStateFactory.Create();
            var uow = new UnitOfWork(entityStateCache);
            var session = new GraphSession(uow, _listeners, _queryExecutorFactory, _repositoryFactory, entityStateCache);
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
