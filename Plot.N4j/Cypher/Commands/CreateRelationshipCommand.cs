using Neo4jClient.Cypher;

namespace Plot.N4j.Cypher.Commands
{
    internal class CreateRelationshipCommand : ICommand
    {
        private readonly ParamSnippet _source;

        private readonly NodeSnippet _destination;

        private readonly string _relationship;

        public CreateRelationshipCommand(ParamSnippet source, NodeSnippet destination, string relationship)
        {
            _source = source;

            _destination = destination;

            _destination = destination;

            _relationship = relationship;
        }

        public ICypherFluentQuery Execute(ICypherFluentQuery query)
        {
            query = query
                .With(new WithSnippet(_source))
                .Match(new MatchNodeSnippet(_destination, new ParamSnippet(_destination.Param, "id")))
                .CreateUnique(new CreateUniqueSnippet(_source, _destination.Param, _relationship))
                .WithParam(new ParamSnippet(_destination.Param, "id"), ProxyUtils.GetEntityId(_destination.Data));
            return query;
        }
    }
}
