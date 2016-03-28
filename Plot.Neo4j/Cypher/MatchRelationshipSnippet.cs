using System.Text;

namespace Plot.Neo4j.Cypher
{
    public class MatchRelationshipSnippet
    {
        private readonly IdentifierNameSnippet _destination;

        private readonly IdentifierNameSnippet _source;

        private readonly RelationshipSnippet _relationship;

        public MatchRelationshipSnippet(IdentifierNameSnippet source, IdentifierNameSnippet destination, RelationshipSnippet relationship)
        {
            _source = source;
            _destination = destination;
            _relationship = relationship;
        }

        public override string ToString()
        {
            var text = new StringBuilder()
                .Append("(")
                .Append(_source)
                .Append(_relationship)
                .Append(_destination)
                .Append(")")
                .ToString();
            return text;
        }
    }
}
