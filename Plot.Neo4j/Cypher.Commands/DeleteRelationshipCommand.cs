using System.Collections.Generic;
namespace Plot.Neo4j.Cypher.Commands
{
    internal class DeleteRelationshipCommand : ICommand
    {
        private readonly IdentifierNameSnippet _source;

        private readonly NodeSnippet _destination;

        private readonly RelationshipSnippet _relationship;

        private readonly bool _deleteOrphan;

        public DeleteRelationshipCommand(IdentifierNameSnippet source, NodeSnippet destination, RelationshipSnippet relationship, bool deleteOrphan)
        {
            _source = source;
            _destination = destination;
            _destination = destination;
            _relationship = relationship;
            _deleteOrphan = deleteOrphan;
        }

        public ICypherFluentQuery Execute(ICypherFluentQuery query)
        {
            query = query
                .With(new WithSnippet(_source))
                .Match(new MatchPropertySnippet(_destination, new IdentifierNameSnippet(_destination.IdentifierName, "id")))
                .Match(new MatchRelationshipSnippet(_source, _destination.IdentifierName, _relationship))
                .WithParam(new IdentifierNameSnippet(_destination.IdentifierName, "id"), ProxyUtils.GetEntityId(_destination.Data))
                .Delete(GetNodesToDelete());
            return query;
        }

        private IdentifierNameSnippet[] GetNodesToDelete()
        {
            var nodes = new List<IdentifierNameSnippet> {_relationship.Identifier};
            if (_deleteOrphan)
            {
                nodes.Add(_destination.IdentifierName);
            }
            return nodes.ToArray();
        }
    }
}
