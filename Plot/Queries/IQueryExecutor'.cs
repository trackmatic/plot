using System.Collections.Generic;

namespace Plot.Queries
{
    public interface IQueryExecutor<TResult> : IQueryExecutor
    {
        IPagedGraphCollection<TResult> ExecuteWithPaging(IGraphSession session, IQuery<TResult> query, bool enlist);

        IEnumerable<TResult> Execute(IGraphSession session, IQuery<TResult> query);
    }
}
