using Neo4jClient.Cypher;

namespace Plot.N4j.Cypher.Commands
{
    internal class DeleteCommand : ICommand
    {
        private readonly NodeSnippet _source;

        private readonly NodeIdSnippet _id;

        public DeleteCommand(NodeSnippet source)
        {
            _source = source;

            _id = new NodeIdSnippet(source);
        }

        public ICypherFluentQuery Execute(ICypherFluentQuery query)
        {
            query = query
                .Match(new MatchNodeSnippet(_source, _id.Param))
                .OptionalMatch($"({_source.Param}-[r]-())")
                .WithParam(_id.Param, ProxyUtils.GetEntityId(_source.Data))
                .Delete(_source.Param, new ParamSnippet("r"));
            return query;
        }
    }
}
