using System.Text;

namespace Plot.N4j.Cypher
{
    public class MatchNodeSnippet
    {
        private readonly NodeSnippet _node;

        private readonly ParamSnippet _value;

        public MatchNodeSnippet(NodeSnippet node, ParamSnippet value)
        {
            _node = node;

            _value = value;
        }

        public override string ToString()
        {
            var text = new StringBuilder()
                .Append("(")
                .Append(_node)
                .Append(" { Id: {")
                .Append(_value)
                .Append("}})")
                .ToString();
            return text;
        }
    }
}
