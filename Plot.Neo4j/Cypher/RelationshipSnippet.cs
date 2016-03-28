using Plot.Metadata;

namespace Plot.Neo4j.Cypher
{
    public class RelationshipSnippet
    {
        private readonly RelationshipMetadata _relationship;

        private readonly IdentifierNameSnippet _identifier;

        public RelationshipSnippet(IdentifierNameSnippet identifier, RelationshipMetadata relationship)
        {
            _identifier = identifier;
            _relationship = relationship;
        }

        public RelationshipSnippet(RelationshipMetadata relationship) 
            : this(null, relationship)
        {
        }

        public override string ToString()
        {
            return _relationship.IsReverse ? $"<{Inner()}" : $"{Inner()}>";
        }

        private string Inner()
        {
            return $"-[{_identifier}:{_relationship.Name}]-";
        }

        public IdentifierNameSnippet Identifier => _identifier;
    }
}
