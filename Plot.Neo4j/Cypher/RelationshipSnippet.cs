using Plot.Metadata;

namespace Plot.Neo4j.Cypher
{
    public class RelationshipSnippet
    {
        private readonly RelationshipMetadata _relationship;

        private RelationshipSnippet(RelationshipMetadata relationship)
        {
            _relationship = relationship;
        }

        public override string ToString()
        {
            if (_relationship.IsReverse)
            {
                return $"<-[:{_relationship.Name}]-";
            }

            return $"-[:{_relationship.Name}]->";
        }

        public static RelationshipSnippet Create(RelationshipMetadata relationship)
        {
            return new RelationshipSnippet(relationship);
        }
    }
}
