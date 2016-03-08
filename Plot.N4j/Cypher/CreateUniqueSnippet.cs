using System.Text;

namespace Plot.N4j.Cypher
{
    public class CreateUniqueSnippet
    {
        private readonly ParamSnippet _source;

        private readonly ParamSnippet _destination;

        private readonly string _relationship;

        public CreateUniqueSnippet(ParamSnippet source, ParamSnippet destination, string relationship)
        {
            _source = source;

            _destination = destination;

            _relationship = relationship;
        }

        public override string ToString()
        {
            var text = new StringBuilder()
                .Append(_source)
                .Append("-[:")
                .Append(_relationship)
                .Append("]->")
                .Append(_destination).ToString();
            return text;
        }
    }
}
