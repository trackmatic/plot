using Neo4jClient;
using Neo4jClient.Cypher;

namespace Octo.N4j.Queries
{
    public interface IQueryBuilder
    {
        ICypherFluentQuery Build(IGraphClient db);
    }
}
