using System.Text;

namespace Plot.Neo4j.Cypher
{
    public class SetIdentifierSnippet
    {
        private readonly IdentifierNameSnippet _param;

        public SetIdentifierSnippet(IdentifierNameSnippet param)
        {
            _param = param;
        }

        public override string ToString()
        {
            var text = new StringBuilder()
                .Append(_param)
                .Append(" = ")
                .Append("{")
                .Append(_param)
                .Append("}")
                .ToString();
            return text;
        }
    }
}
