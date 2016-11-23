using System.Collections.Generic;
using System.Linq;
using Plot.Queries;

namespace Plot.Neo4j.Cypher
{
    public class OrderByHelper<TResult, TAggregate>
    {
        private bool _started;

        private ICypherQuery<TResult> Append(ICypherQuery<TResult> cypher, IEnumerable<Order<TResult>> orders)
        {
            foreach (var order in orders)
            {
                if (_started)
                {
                    cypher = order.Continue(cypher);
                }
                else
                {
                    cypher = order.Start(cypher);
                    _started = true;
                }
            }
            return cypher;
        }

        public static ICypherQuery<TResult> OrderBy(ICypherQuery<TResult> cypher, IQuery<TAggregate> query)
        {
            if (query.OrderBy == null)
            {
                return cypher;
            }
            var helper = new OrderByHelper<TResult, TAggregate>();
            return helper.Append(cypher, query.OrderBy.Select(x => new Order<TResult>(x)));
        }
    }
}
