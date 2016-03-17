using Neo4jClient.Cypher;

namespace Plot.Neo4j.Cypher.Commands
{
    public interface ICommand
    {
        ICypherFluentQuery Execute(ICypherFluentQuery query);
    }
}
