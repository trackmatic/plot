using System.Collections.Concurrent;
using System.Collections.Generic;
using Neo4jClient;

namespace Plot.Neo4j
{
    public class CypherTransactionFactory : ICypherTransactionFactory
    {
        private readonly IGraphClient _db;

        private readonly IDictionary<IGraphSession, ICypherTransaction> _transactions;

        public CypherTransactionFactory(IGraphClient db)
        {
            _db = db;
            _transactions = new ConcurrentDictionary<IGraphSession, ICypherTransaction>();
        }

        public ICypherTransaction Create(IGraphSession session)
        {
            if (_transactions.ContainsKey(session))
            {
                return _transactions[session];
            }
            var transaction = new CypherTransaction(_db);
            _transactions.Add(session, transaction);
            session.Flushed += OnFlushed;
            session.Disposed += OnDisposed;
            return transaction;
        }

        private void OnDisposed(object sender, GraphSessionDisposedEventArgs e)
        {
            _transactions.Remove(e.Session);
            e.Session.Disposed -= OnDisposed;
            e.Session.Flushed -= OnFlushed;
        }

        private void OnFlushed(object sender, GraphSessionFlushedEventArgs e)
        {
            var transaction = _transactions[e.Session];
            transaction.Commit();
        }
    }
}
