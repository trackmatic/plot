using System.Collections.Generic;

namespace Plot.Neo4j.Cypher
{
    public class QueryBuilder : IQueryBuilder
    {
        private readonly IEnumerable<IQueryBuilderElement> _elements;

        public QueryBuilder(IEnumerable<IQueryBuilderElement> elements)
        {
            _elements = elements;
        }

        public ICypherQuery Build(ICypherQuery query)
        {
            var current = query;
            foreach (var element in _elements)
            {
                current = element.Append(current);
            }
            return current;
        }

        public static ICypherQuery Create(ICypherQuery query, IEnumerable<IQueryBuilderElement> elements)
        {
            return new QueryBuilder(elements).Build(query);
        }
    }
}
