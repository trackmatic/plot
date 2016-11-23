using System;

namespace Plot.Neo4j.Cypher.Commands
{
    internal class CreateNodeCommand : ICommand
    {
        private readonly Node _node;
        private readonly Func<object> _factory;
        
        public CreateNodeCommand(Node node, Func<object> factory)
        {
            _node = node;
            _factory = factory;
        }

        public ICypherQuery Execute(ICypherQuery query)
        {
            var node = StatementFactory.Parameter(_node);
            var id = StatementFactory.IdParameter(_node);
            var merge = StatementFactory.Merge(_node, id);
            var set = StatementFactory.Set(_node, node);
            query = query
                .Merge(merge)
                .OnCreate()
                .Set(set)
                .OnMatch()
                .Set(set)
                .WithParam(node, _factory().ToDictionary())
                .WithParam(id, ProxyUtils.GetEntityId(_node));
            return query;
        }
    }
}
