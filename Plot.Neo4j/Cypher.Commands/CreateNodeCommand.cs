using System;

namespace Plot.Neo4j.Cypher.Commands
{
    internal class CreateNodeCommand : ICommand
    {
        private readonly PropertyIdentifierSnippet _id;

        private readonly NodeSnippet _source;

        private readonly Func<object> _factory;
        
        public CreateNodeCommand(NodeSnippet source, Func<object> factory)
        {
            _source = source;
            _id = new PropertyIdentifierSnippet(source);
            _factory = factory;
        }

        public ICypherFluentQuery Execute(ICypherFluentQuery query)
        {
            query = query
                .Merge(new MatchPropertySnippet(_source, _id))
                .OnCreate()
                .Set(new SetIdentifierSnippet(_source.IdentifierName))
                .OnMatch()
                .Set(new SetIdentifierSnippet(_source.IdentifierName))
                .WithParam(_id, ProxyUtils.GetEntityId(_source.Data))
                .WithParam(_source.IdentifierName, _factory().ToDictionary());
            return query;
        }
    }
}
