using System.Collections.Generic;
using Octo.Core.Queries;

namespace Octo.Core
{

    public interface IRepository<T> : IRepository
    {
        void Store(T item);

        void Delete(T item);

        IEnumerable<T> Get(params string[] id);

        IPagedGraphCollection<T> Query(IQueryExecutor<T> queryExecutor, IQuery<T> query);
    }
}
