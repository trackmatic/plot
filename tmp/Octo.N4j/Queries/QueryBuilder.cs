using System.Collections.Generic;
using System.Linq;
using Neo4jClient;
using Neo4jClient.Cypher;

namespace Octo.N4j.Queries
{
    public class QueryBuilder : IQueryBuilder
    {
        private readonly IEnumerable<IQueryBuilderElement> _elements;

        public QueryBuilder(IEnumerable<IQueryBuilderElement> elements)
        {
            _elements = elements;
        }

        public ICypherFluentQuery Build(IGraphClient db)
        {
            return _elements.Aggregate(db.Cypher, (current, element) => element.Append(current));
        }

        public static ICypherFluentQuery Create(IGraphClient db, IEnumerable<IQueryBuilderElement> elements)
        {
            return new QueryBuilder(elements).Build(db);
        }
    }
}
