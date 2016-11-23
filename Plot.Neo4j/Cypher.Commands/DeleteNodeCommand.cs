namespace Plot.Neo4j.Cypher.Commands
{
    internal class DeleteNodeCommand : ICommand
    {
        private readonly Node _node;

        public DeleteNodeCommand(Node node)
        {
            _node = node;
        }

        public ICypherQuery Execute(ICypherQuery query)
        {
            query = query
                .Match(StatementFactory.Match(_node, StatementFactory.IdParameter(_node)))
                .OptionalMatch($"({StatementFactory.ExistingNode(_node)}-[r]-())")
                .WithParam(StatementFactory.IdParameter(_node), _node.Id)
                .Delete($"r, {StatementFactory.ExistingNode(_node)}");
            return query;
        }
    }
}
