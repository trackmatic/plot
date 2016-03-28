using System.Text;

namespace Plot.Neo4j.Cypher
{
    public class MatchPropertySnippet
    {
        private readonly NodeSnippet _nodeSnippet;

        private readonly IdentifierNameSnippet _identifierNameSnippet;

        private readonly string _propertyName;

        public MatchPropertySnippet(NodeSnippet nodeSnippet, IdentifierNameSnippet identifierNameSnippet, string propertyName = "Id")
        {
            _nodeSnippet = nodeSnippet;
            _identifierNameSnippet = identifierNameSnippet;
            _propertyName = propertyName;
        }

        public override string ToString()
        {
            var text = new StringBuilder()
                .Append("(")
                .Append(_nodeSnippet)
                .Append(" {")
                .Append(_propertyName)
                .Append(":{")
                .Append(_identifierNameSnippet)
                .Append("}})")
                .ToString();
            return text;
        }
    }
}
