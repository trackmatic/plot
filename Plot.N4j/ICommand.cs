using Neo4jClient.Cypher;

namespace Plot.N4j
{
    public interface ICommand
    {
        ICypherFluentQuery Execute(ICypherFluentQuery query);
    }
}
