using Plot.Metadata;

namespace Plot.Neo4j.Queries
{

    public class RelationshipSyntax
    {
        private readonly RelationshipMetadata _relationship;

        private RelationshipSyntax(RelationshipMetadata relationship)
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

        public static RelationshipSyntax Create(RelationshipMetadata relationship)
        {
            return new RelationshipSyntax(relationship);
        }
    }
}
