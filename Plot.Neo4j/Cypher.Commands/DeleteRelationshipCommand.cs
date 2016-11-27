using System;
using Plot.Metadata;

namespace Plot.Neo4j.Cypher.Commands
{
    internal class DeleteRelationshipCommand : ICommand
    {
        private readonly Node _source;
        private readonly Node _destination;
        private readonly RelationshipMetadata _relationship;
        private readonly string _relationshipName;

        public DeleteRelationshipCommand(Node source, Node destination, RelationshipMetadata relationship)
        {
            _source = source;
            _destination = destination;
            _relationship = relationship;
            _relationshipName = StatementFactory.MakeSafe(Guid.NewGuid());
        }

        public ICypherQuery Execute(ICypherQuery query)
        {
            query = query
                .With(StatementFactory.With(_source))
                .Match(StatementFactory.Match(_destination, StatementFactory.IdParameter(_destination)))
                .Match(StatementFactory.Relationship(_source, _destination, _relationship, _relationshipName))
                .WithParam(StatementFactory.IdParameter(_destination), _destination.Id)
                .Delete(GetNodesToDelete());
            return query;
        }

        private string GetNodesToDelete()
        {
            var statment = _relationshipName;
            if (_relationship.DeleteOrphan)
            {
                statment += $", {StatementFactory.ExistingNode(_destination)}";
            }
            return statment;
        }
    }
}
