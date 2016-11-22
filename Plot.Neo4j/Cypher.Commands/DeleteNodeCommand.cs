namespace Plot.Neo4j.Cypher.Commands
{
    internal class DeleteNodeCommand : ICommand
    {
        private readonly NodeSnippet _source;

        private readonly PropertyIdentifierSnippet _nodeIdentifierSnippet;

        public DeleteNodeCommand(NodeSnippet source)
        {
            _source = source;
            _nodeIdentifierSnippet = new PropertyIdentifierSnippet(source);
        }

        public ICypherFluentQuery Execute(ICypherFluentQuery query)
        {
            query = query
                .Match(new MatchPropertySnippet(_source, _nodeIdentifierSnippet))
                .OptionalMatch($"({_source.IdentifierName}-[r]-())")
                .WithParam(_nodeIdentifierSnippet, ProxyUtils.GetEntityId(_source.Data))
                .Delete(_source.IdentifierName, new IdentifierNameSnippet("r"));
            return query;
        }
    }
}
