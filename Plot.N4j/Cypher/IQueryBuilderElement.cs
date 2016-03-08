using Neo4jClient.Cypher;

namespace Plot.N4j.Cypher
{
    public interface IQueryBuilderElement
    {
        ICypherFluentQuery Append(ICypherFluentQuery cypher);
    }
}
