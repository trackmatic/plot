using Neo4jClient;
using Neo4jClient.Cypher;

namespace Plot.N4j.Cypher
{
    public interface IQueryBuilder
    {
        ICypherFluentQuery Build(IGraphClient db);
    }
}
