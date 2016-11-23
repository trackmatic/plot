namespace Plot.Neo4j.Cypher.Commands
{
    internal class DeleteNodeCommand : ICommand
    {
        private readonly Entity _entity;

        public DeleteNodeCommand(Entity entity)
        {
            _entity = entity;
        }

        public ICypherFluentQuery Execute(ICypherFluentQuery query)
        {
            query = query
                .Match(StatementFactory.Match(_entity, StatementFactory.IdParameter(_entity)))
                .OptionalMatch($"({StatementFactory.ExistingNode(_entity)}-[r]-())")
                .WithParam(StatementFactory.IdParameter(_entity), _entity.Id)
                .Delete($"r, {StatementFactory.ExistingNode(_entity)}");
            return query;
        }
    }
}
