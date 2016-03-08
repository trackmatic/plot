using Neo4jClient.Cypher;

namespace Plot.N4j.Cypher.Commands
{
    public class DeleteOrphanCommand : ICommand
    {
        private readonly NodeSnippet _source;

        private readonly NodeIdSnippet _id;

        public DeleteOrphanCommand(NodeSnippet source)
        {
            _source = source;

            _id = new NodeIdSnippet(source);
        }

        public ICypherFluentQuery Execute(ICypherFluentQuery query)
        {
            query = query
                .With(new WithSnippet(_source.Param))
                .OptionalMatch($"({_source.Param}-[r]-())")
                .WithParam(_id.Param, ProxyUtils.GetEntityId(_source.Data))
                .Delete(_source.Param, new ParamSnippet("r"));
            return query;
        }
    }
}
