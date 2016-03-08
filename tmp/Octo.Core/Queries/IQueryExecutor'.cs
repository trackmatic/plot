using System.Collections.Generic;

namespace Octo.Core.Queries
{
    public interface IQueryExecutor<TResult> : IQueryExecutor
    {
        IPagedGraphCollection<TResult> Execute(IQuery<TResult> query);

        IEnumerable<TResult> Execute(IUnitOfWork uow, IQuery<TResult> query);
    }
}
