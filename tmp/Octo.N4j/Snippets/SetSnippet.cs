using System.Text;

namespace Octo.N4j.Snippets
{
    public class SetSnippet
    {
        private readonly ParamSnippet _param;

        public SetSnippet(ParamSnippet param)
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
