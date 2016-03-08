using System;
using Neo4jClient.Cypher;

namespace Plot.N4j.Cypher.Commands
{
    internal class CreateCommandBase : ICommand
    {
        private readonly NodeIdSnippet _id;

        private readonly NodeSnippet _source;

        private readonly Func<object> _factory;
        
        public CreateCommandBase(NodeSnippet source, Func<object> factory)
        {
            _source = source;
            _id = new NodeIdSnippet(source);
            _factory = factory;
        }

        public ICypherFluentQuery Execute(ICypherFluentQuery query)
        {
            query = query
                .Merge(new MatchNodeSnippet(_source, _id.Param))
                .OnCreate()
                .Set(new SetSnippet(_source.Param))
                .OnMatch()
                .Set(new SetSnippet(_source.Param))
                .WithParam(_id.Param, ProxyUtils.GetEntityId(_source.Data))
                .WithParam(_source.Param, _factory());
            return query;
        }
    }
}
