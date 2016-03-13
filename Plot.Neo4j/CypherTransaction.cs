using System;
using System.Collections.Generic;
using System.Text;
using Neo4jClient;
using Neo4jClient.Cypher;

namespace Plot.Neo4j
{
    public class CypherTransaction : ICypherTransaction
    {
        private readonly IGraphClient _db;

        private readonly List<ICypherFluentQuery> _items;

        public CypherTransaction(IGraphClient db)
        {
            _db = db;
            _items = new List<ICypherFluentQuery>();
        }

        public void Commit()
        {
            foreach (var item in _items)
            {
                Log(item);
                item.ExecuteWithoutResults();
            }
            _items.Clear();
        }

        public void Enlist(IMapper mapper, Func<ICypherFluentQuery, ICypherFluentQuery> callback)
        {
            var query = _db.Cypher;

            query = callback(query);

            _items.Add(query);
        }

        private void Log(ICypherFluentQuery query)
        {
            var builder = new StringBuilder();
            builder.AppendLine("-----------------------START-----------------------");
            builder.AppendLine(query.Query.DebugQueryText);
            builder.AppendLine("-----------------------END-------------------------");
            Console.WriteLine(builder);
        }
    }
}
