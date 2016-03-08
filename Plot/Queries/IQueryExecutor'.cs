using System.Collections.Generic;

namespace Plot.Queries
{
    public interface IQueryExecutor<TResult> : IQueryExecutor
    {
        IPagedGraphCollection<TResult> Execute(IQuery<TResult> query);

        IEnumerable<TResult> Execute(IUnitOfWork uow, IQuery<TResult> query);
    }
}
