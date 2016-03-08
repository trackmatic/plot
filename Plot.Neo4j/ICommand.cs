using Neo4jClient.Cypher;

namespace Plot.Neo4j
{
    public interface ICommand
    {
        ICypherFluentQuery Execute(ICypherFluentQuery query);
    }
}
