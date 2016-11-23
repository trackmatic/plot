using Plot.Neo4j.Cypher;

namespace Plot.Sample.Data.Params
{
    public class Parameters : IQueryBuilderElement
    {
        private readonly Parameter[] _parameters;

        public Parameters(params Parameter[] parameters)
        {
            _parameters = parameters;
        }

        public ICypherQuery Append(ICypherQuery cypher)
        {
            foreach (var parameter in _parameters)
            {
                cypher = parameter.Append(cypher);
            }
            return cypher;
        }
    }
}
