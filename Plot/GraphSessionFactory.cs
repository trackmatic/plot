using System.Collections.Generic;
using Plot.Queries;

namespace Plot
{
    public class GraphSessionFactory : IGraphSessionFactory
    {
        private readonly IQueryExecutorFactory _queryExecutorFactory;

        private readonly IRepositoryFactory _repositoryFactory;

        private readonly List<IListener> _listeners;

        public GraphSessionFactory(IQueryExecutorFactory queryExecutorFactory, IRepositoryFactory repositoryFactory)
        {
            _queryExecutorFactory = queryExecutorFactory;
            _repositoryFactory = repositoryFactory;
            _listeners = new List<IListener>();
        }

        public IGraphSession OpenSession()
        {
            return new GraphSession(new UnitOfWork(), _listeners, _queryExecutorFactory, _repositoryFactory);
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
