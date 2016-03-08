using Neo4jClient.Cypher;

namespace Octo.N4j.Queries
{
    public interface IQueryBuilderElement
    {
        ICypherFluentQuery Append(ICypherFluentQuery cypher);
    }
}
