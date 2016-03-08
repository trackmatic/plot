using Neo4jClient.Cypher;

namespace Octo.N4j
{
    public interface ICommand
    {
        ICypherFluentQuery Execute(ICypherFluentQuery query);
    }
}
