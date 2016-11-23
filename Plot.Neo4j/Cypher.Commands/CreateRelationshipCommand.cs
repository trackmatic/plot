using Plot.Metadata;

namespace Plot.Neo4j.Cypher.Commands
{
    internal class CreateRelationshipCommand : ICommand
    {
        private readonly RelationshipMetadata _relationship;
        private readonly Entity _source;
        private readonly Entity _destination;

        public CreateRelationshipCommand(Entity source, Entity destination, RelationshipMetadata relationship)
        {
            _source = source;
            _destination = destination;
            _relationship = relationship;
        }

        public ICypherFluentQuery Execute(ICypherFluentQuery query)
        {
            query
                .With(StatementFactory.With(_source))
                .Match(StatementFactory.Match(_destination, StatementFactory.IdParameter(_destination)))
                .CreateUnique(StatementFactory.Relationship(_source, _destination, _relationship))
                .WithParam(StatementFactory.IdParameter(_destination), _destination.Id);
            return query;
        }
    }
}
