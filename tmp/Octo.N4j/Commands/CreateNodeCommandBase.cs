using Neo4jClient.Cypher;
using Octo.Core;
using Octo.N4j.Snippets;

namespace Octo.N4j.Commands
{
    public abstract class CreateCommandBase<TSource> : ICommand
    {
        private readonly NodeIdSnippet<TSource> _id;

        private readonly NodeSnippet<TSource> _source;
        
        protected CreateCommandBase(NodeSnippet<TSource> source)
        {
            _source = source;

            _id = new NodeIdSnippet<TSource>(source);
        }

        public ICypherFluentQuery Execute(ICypherFluentQuery query)
        {
            query = query
                .Merge(new MatchNodeSnippet(_source, _id.Param))
                .OnCreate()
                .Set(new SetSnippet(_source.Param))
                .OnMatch()
                .Set(new SetSnippet(_source.Param))
                .WithParam(_id.Param, EntityUtils.GetEntityId(_source.Data))
                .WithParam(_source.Param, Data());
            return query;
        }
        
        protected abstract object Data();
    }
}
