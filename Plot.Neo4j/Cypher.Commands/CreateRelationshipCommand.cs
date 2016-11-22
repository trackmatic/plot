namespace Plot.Neo4j.Cypher.Commands
{
    internal class CreateRelationshipCommand : ICommand
    {
        private readonly IdentifierNameSnippet _source;

        private readonly NodeSnippet _destination;

        private readonly RelationshipSnippet _relationship;

        public CreateRelationshipCommand(IdentifierNameSnippet source, NodeSnippet destination, RelationshipSnippet relationship)
        {
            _source = source;
            _destination = destination;
            _relationship = relationship;
        }

        public ICypherFluentQuery Execute(ICypherFluentQuery query)
        {
            query = query
                .With(new WithSnippet(_source))
                .Match(new MatchPropertySnippet(_destination, new IdentifierNameSnippet(_destination.IdentifierName, "id")))
                .CreateUnique(new MatchRelationshipSnippet(_source, _destination.IdentifierName, _relationship))
                .WithParam(new IdentifierNameSnippet(_destination.IdentifierName, "id"), ProxyUtils.GetEntityId(_destination.Data));
            return query;
        }
    }
}
