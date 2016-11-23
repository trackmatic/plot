namespace Plot.Neo4j.Cypher.Commands
{
    public interface ICommand
    {
        ICypherQuery Execute(ICypherQuery query);
    }
}
