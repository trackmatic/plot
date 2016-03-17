using Plot.Queries;

namespace Plot.Neo4j.Queries
{
    public interface ICypherQueryResult<TAggregate> : IQueryResult
    {
        void Map(TAggregate aggregate);

        TAggregate Create();
    }
}
