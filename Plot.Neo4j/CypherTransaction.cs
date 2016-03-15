using System;
using System.Collections.Generic;
using Neo4jClient.Cypher;
using Neo4jClient.Transactions;
using Plot.Logging;

namespace Plot.Neo4j
{
    public class CypherTransaction : ICypherTransaction
    {
        private readonly ITransactionalGraphClient _db;

        private readonly List<ICypherFluentQuery> _items;

        private bool _disposed;

        private readonly ITransaction _transaction;

        private readonly Guid _id;

        private readonly ILogger _logger;

        public CypherTransaction(ITransactionalGraphClient db, ILogger logger)
        {
            _id = Guid.NewGuid();
            _db = db;
            _items = new List<ICypherFluentQuery>();
            _transaction = _db.BeginTransaction();
            _logger = logger;
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
            _logger.Info(query.Query.DebugQueryText);
        }

        public override string ToString()
        {
            return $"transaction:{_id}";
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }
            _disposed = true;
            _transaction.Commit();
        }
    }
}
