using Neo4jClient.Cypher;

namespace Plot.Neo4j.Cypher
{
    public interface IQueryBuilderElement
    {
        ICypherFluentQuery Append(ICypherFluentQuery cypher);
    }
}
