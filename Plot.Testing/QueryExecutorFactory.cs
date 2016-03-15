using System;
using Plot.Queries;

namespace Plot.Testing
{
    public class QueryExecutorFactory : IQueryExecutorFactory
    {
        public IQueryExecutor<TResult> Create<TResult>(IQuery<TResult> query, IGraphSession session)
        {
            throw new NotImplementedException();
        }
    }
}
