using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Neo4j.Driver.V1;
using Plot.Neo4j.Cypher;
using ILogger = Plot.Logging.ILogger;

namespace Plot.Neo4j
{
    public class CypherTransactionFactory : ICypherTransactionFactory
    {
        private readonly IDriver _db;
        private readonly IDictionary<IGraphSession, ICypherTransaction> _transactions;
        private readonly ILogger _logger;

        public CypherTransactionFactory(IDriver db, ILogger logger)
        {
            _db = db;
            _logger = logger;
            _transactions = new ConcurrentDictionary<IGraphSession, ICypherTransaction>();
        }

        public ICypherTransaction Create(IGraphSession session)
        {
            if (_transactions.ContainsKey(session))
            {
                return _transactions[session];
            }
            var transaction = new CypherTransaction(_db, _logger);
            _transactions.Add(session, transaction);
            session.Flushed += OnFlushed;
            session.Disposed += OnDisposed;
            return transaction;
        }

        public IList<T> Run<T>(ICypherQuery<T> query)
        {
            using (var session = _db.Session())
            {
                var results = session.Run(query.Statement, query.Parameters).ToList();
                return results.Select(query.Map).ToList();
            }
        }

        public ILogger Logger => _logger;

        private void OnDisposed(object sender, GraphSessionDisposedEventArgs e)
        {
            try
            {
                var transaction = _transactions[e.Session];
                transaction.Dispose();
                _logger.Info($"Disposing transaction {transaction}");
            }
            finally
            {
                _transactions.Remove(e.Session);
                e.Session.Disposed -= OnDisposed;
                e.Session.Flushed -= OnFlushed;
            }
        }

        private void OnFlushed(object sender, GraphSessionFlushedEventArgs e)
        {
            var transaction = _transactions[e.Session];
            transaction.Commit();
            _logger.Info($"Flushing transaction {transaction}");
        }
    }
}
