namespace Plot.Neo4j.Cypher
{
    public class WithSnippet
    {
        private readonly ParamSnippet _param;

        public WithSnippet(ParamSnippet param)
        {
            _param = param;
        }

        public override string ToString()
        {
            return _param.ToString();
        }
    }
}
