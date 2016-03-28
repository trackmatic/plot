namespace Plot.Neo4j.Cypher
{
    public class WithSnippet
    {
        private readonly IdentifierNameSnippet _param;

        public WithSnippet(IdentifierNameSnippet param)
        {
            _param = param;
        }

        public override string ToString()
        {
            return _param.ToString();
        }
    }
}
