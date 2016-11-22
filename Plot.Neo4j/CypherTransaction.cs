using System;
using System.Collections.Generic;
using Neo4j.Driver.V1;
using Plot.Neo4j.Cypher;
using ILogger = Plot.Logging.ILogger;

namespace Plot.Neo4j
{
    public class CypherTransaction : ICypherTransaction
    {
        private readonly IDriver _db;
        private readonly List<ICypherFluentQuery> _items;
        private bool _disposed;
        private readonly Guid _id;
        private readonly ILogger _logger;

        public CypherTransaction(IDriver db, ILogger logger)
        {
            _id = Guid.NewGuid();
            _db = db;
            _items = new List<ICypherFluentQuery>();
            _logger = logger;
        }

        public void Commit()
        {
            using(var session = _db.Session())
            using (var transaction = session.BeginTransaction())
            {
                foreach (var item in _items)
                {
                    Log(item);
                    transaction.Run(item.Statement, item.Parameters);
                }
                _items.Clear();
                transaction.Success();
            }
        }

        public void Enlist(IMapper mapper, Func<ICypherFluentQuery, ICypherFluentQuery> callback)
        {
            var query = CreateFluentQuery(mapper.Type);
            query = callback(query);
            _items.Add(query);
        }

        private void Log(ICypherFluentQuery query)
        {
            _logger.Info(query.GetDebugText());
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
        }

        private ICypherFluentQuery CreateFluentQuery(Type type)
        {
            return (ICypherFluentQuery)Activator.CreateInstance(typeof(CypherFluentQuery<>).MakeGenericType(type));
        }
    }
}
