using Neo4jClient.Cypher;
using Octo.Core;
using Octo.N4j.Snippets;

namespace Octo.N4j.Commands
{
    public class DeleteCommandBase<TSource> : ICommand
    {
        private readonly NodeSnippet<TSource> _source;

        private readonly NodeIdSnippet<TSource> _id;

        public DeleteCommandBase(NodeSnippet<TSource> source)
        {
            _source = source;

            _id = new NodeIdSnippet<TSource>(source);
        }

        public ICypherFluentQuery Execute(ICypherFluentQuery query)
        {
            query = query
                .Match(new MatchNodeSnippet(_source, _id.Param))
                .OptionalMatch($"({_source.Param}-[r]-())")
                .WithParam(_id.Param, EntityUtils.GetEntityId(_source.Data))
                .Delete(_source.Param, new ParamSnippet("r"));
            return query;
        }
    }
}
