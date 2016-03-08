using Neo4jClient;
using Plot.Queries;

namespace Plot.N4j.Queries
{
    public abstract class GetQueryExecutorBase<TAggregate, TDataset> : AbstractQueryExecutor<TAggregate, TDataset, GetAbstractQuery<TAggregate>>
        where TDataset : IQueryResult
    {
        protected GetQueryExecutorBase(GraphClient db)
            : base(db)
        {
        }
    }
}
