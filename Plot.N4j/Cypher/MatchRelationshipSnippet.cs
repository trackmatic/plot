using System.Text;

namespace Plot.N4j.Cypher
{
    public class MatchRelationshipSnippet
    {
        private readonly ParamSnippet _destination;

        private readonly ParamSnippet _source;

        private readonly ParamSnippet _name;

        private readonly string _relationship;

        public MatchRelationshipSnippet(ParamSnippet source, ParamSnippet destination, ParamSnippet name, string relationship)
        {
            _source = source;

            _name = name;

            _destination = destination;

            _relationship = relationship;
        }

        public override string ToString()
        {
            var text = new StringBuilder()
                .Append("(")
                .Append(_source)
                .Append("-[")
                .Append(_name)
                .Append(":")
                .Append(_relationship)
                .Append("]->")
                .Append(_destination)
                .Append(")")
                .ToString();
            return text;
        }
    }
}
