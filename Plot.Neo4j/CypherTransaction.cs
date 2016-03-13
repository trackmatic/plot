using System;
using System.Collections.Generic;
using System.Threading;
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
                Console.ReadLine();
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
            Console.WriteLine("-----------------------START-----------------------");
            var debug = query.Query.DebugQueryText;
            Console.WriteLine(debug);
            Console.WriteLine("-----------------------END-------------------------");
        }
    }
}
