using Plot.Queries;

namespace Plot.Neo4j.Queries
{
    public abstract class AbstractCypherQueryResult<TAggregate> : AbstractQueryResult, ICypherQueryResult<TAggregate>
    {
        public abstract void Map(TAggregate aggregate);

        public abstract TAggregate Create();
    }
}
