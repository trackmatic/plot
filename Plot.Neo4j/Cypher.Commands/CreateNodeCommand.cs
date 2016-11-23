using System;
using Plot.Metadata;

namespace Plot.Neo4j.Cypher.Commands
{
    internal class CreateNodeCommand : ICommand
    {
        private readonly Entity _entity;
        private readonly Func<object> _factory;
        
        public CreateNodeCommand(Entity entity, Func<object> factory)
        {
            _entity = entity;
            _factory = factory;
        }

        public ICypherFluentQuery Execute(ICypherFluentQuery query)
        {
            var node = StatementFactory.Parameter(_entity);
            var id = StatementFactory.IdParameter(_entity);
            var merge = StatementFactory.Merge(_entity, id);
            var set = StatementFactory.Set(_entity, node);
            query = query
                .Merge(merge)
                .OnCreate()
                .Set(set)
                .OnMatch()
                .Set(set)
                .WithParam(node, _factory().ToDictionary())
                .WithParam(id, ProxyUtils.GetEntityId(_entity));
            return query;
        }
    }
}
