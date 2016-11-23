using Plot.Metadata;

namespace Plot.Neo4j.Cypher.Commands
{
    internal class DeleteRelationshipCommand : ICommand
    {
        private readonly Entity _source;
        private readonly Entity _destination;
        private readonly RelationshipMetadata _relationship;

        public DeleteRelationshipCommand(Entity source, Entity destination, RelationshipMetadata relationship)
        {
            _source = source;
            _destination = destination;
            _relationship = relationship;
        }

        public ICypherFluentQuery Execute(ICypherFluentQuery query)
        {
            query = query
                .With(StatementFactory.With(_source))
                .Match(StatementFactory.Match(_destination, StatementFactory.IdParameter(_destination)))
                .Match(StatementFactory.Relationship(_source, _destination, _relationship, "r"))
                .WithParam(StatementFactory.IdParameter(_destination), _destination.Id)
                .Delete(GetNodesToDelete());
            return query;
        }

        private string GetNodesToDelete()
        {
            var statment = $"r, {StatementFactory.ExistingNode(_source)}";
            if (_relationship.DeleteOrphan)
            {
                statment += $", {StatementFactory.ExistingNode(_destination)}";
            }
            return statment;
        }
    }
}
