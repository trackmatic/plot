using System.Collections.Generic;
using Neo4jClient.Cypher;

namespace Plot.Neo4j.Cypher.Commands
{
    internal class DeleteRelationshipCommand : ICommand
    {
        private readonly ParamSnippet _source;

        private readonly NodeSnippet _destination;

        private readonly string _relationship;

        private readonly ParamSnippet _relationshipName;

        private readonly bool _deleteOrphan;

        public DeleteRelationshipCommand(ParamSnippet source, NodeSnippet destination, string relationship, bool deleteOrphan)
        {
            _source = source;
            _destination = destination;
            _destination = destination;
            _relationship = relationship;
            _relationshipName = new ParamSnippet(source, "rel");
            _deleteOrphan = deleteOrphan;
        }

        public ICypherFluentQuery Execute(ICypherFluentQuery query)
        {
            query = query
                .With(new WithSnippet(_source))
                .Match(new MatchNodeSnippet(_destination, new ParamSnippet(_destination.Param, "id")))
                .Match(new MatchRelationshipSnippet(_source, _destination.Param, _relationshipName, _relationship))
                .WithParam(new ParamSnippet(_destination.Param, "id"), ProxyUtils.GetEntityId(_destination.Data))
                .Delete(GetNodesToDelete());
            return query;
        }

        private ParamSnippet[] GetNodesToDelete()
        {
            var nodes = new List<ParamSnippet>();
            nodes.Add(_relationshipName);
            if (_deleteOrphan)
            {
                nodes.Add(_destination.Param);
            }
            return nodes.ToArray();
        }
    }
}
