using Neo4jClient;
using Octo.Core.Queries;

namespace Octo.N4j.Queries
{
    public abstract class AbstractGetQueryExecutor<TAggregate, TDataset> : AbstractQueryExecutor<TAggregate, TDataset, GetQuery<TAggregate>>
        where TDataset : IQueryResult
    {
        protected AbstractGetQueryExecutor(GraphClient db)
            : base(db)
        {
        }
    }
}
