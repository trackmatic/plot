using Neo4jClient;
using Neo4jClient.Cypher;

namespace Plot.Neo4j.Cypher
{
    public interface IQueryBuilder
    {
        ICypherFluentQuery Build(IGraphClient db);
    }
}
